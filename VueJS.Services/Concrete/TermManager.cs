using AutoMapper;
using VueJS.Data.Abstract;
using VueJS.Entities.Concrete;
using VueJS.Entities.Dtos;
using VueJS.Services.Abstract;
using VueJS.Services.Utilities;
using VueJS.Shared.Utilities.Results.Concrete;
using VueJS.Shared.Utilities.Results.Abstract;
using VueJS.Shared.Utilities.Results.ComplexTypes;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using VueJS.Entities.ComplexTypes;
using VueJS.Services.Helper.Abstract;

namespace VueJS.Services.Concrete
{
    public class TermManager : ManagerBase, ITermService
    {
        public TermManager(IUnitOfWork unitOfWork, IMapper mapper, IExtensionsHelper extensionsHelper) : base(unitOfWork, mapper, extensionsHelper) { }

        public async Task<IDataResult<TermDto>> GetAsync(int termId)
        {
            var term = await UnitOfWork.Terms.GetAsync(t => t.Id == termId);
            if (term == null) return new DataResult<TermDto>(ResultStatus.Error, Messages.NotFound(term.TermType, false), null);
            return new DataResult<TermDto>(ResultStatus.Success, new TermDto { Term = term });
        }

        public async Task<IDataResult<TermDto>> GetAsync(ObjectType termType, string termSlug)
        {
            Term term = null;
            switch (termType)
            {
                case ObjectType.category:
                    term = await UnitOfWork.Terms.GetAsync(c => c.TermType == ObjectType.category && c.Slug == termSlug, c => c.Parent, c => c.Children);
                    
                    Term termParent = (Term)await ExtensionsHelper.GetParentsAsync(termType, term);
                    term.Parents = termParent.Parents;
                    break;
                case ObjectType.tag:
                    term = await UnitOfWork.Terms.GetAsync(c => c.TermType == ObjectType.tag && c.Slug == termSlug);
                    break;
            }

            if (term == null) return new DataResult<TermDto>(ResultStatus.Error, Messages.NotFound(termType, false), null);
            return new DataResult<TermDto>(ResultStatus.Success, new TermDto { Term = term });
        }

        public async Task<IDataResult<TermListDto>> GetAllAsync(ObjectType termType)
        {
            var terms = await UnitOfWork.Terms.GetAllAsync(t => t.TermType == termType, t => t.Parent, t => t.Children, t => t.PostTerms);
            if (terms.Count < 1) return new DataResult<TermListDto>(ResultStatus.Error, Messages.NotFound(termType, true), null);

            foreach (var term in terms)
            {
                Term termParent = (Term)await ExtensionsHelper.GetParentsAsync(termType, term);
                term.Parents = termParent.Parents;
            }
            return new DataResult<TermListDto>(ResultStatus.Success, new TermListDto { Terms = terms });
        }

        public async Task<IDataResult<TermListDto>> GetAllParentAsync(int? termId)
        {
            IList<Term> terms = null;
            if (termId == null)
            {
                terms = await UnitOfWork.Terms.GetAllAsync(t => t.TermType == ObjectType.category);
            }
            else
            {
                var currentTerm = await UnitOfWork.Terms.GetAsync(t => t.Id == termId.Value, c => c.Children, c => c.Parent, c => c.Parents);

                if (currentTerm != null)
                {
                    Term termParent = (Term)await ExtensionsHelper.GetParentsAsync(currentTerm.TermType, currentTerm);
                    currentTerm.Parents = termParent.Parents;

                    List<int> parentIds = new List<int>();
                    foreach (var par in currentTerm.Parents)
                    {
                        parentIds.Add(par.Id);
                    }

                    List<int> childIds = new List<int>();
                    foreach (var child in currentTerm.Children)
                    {
                        childIds.Add(child.Id);
                    }
                    terms = await UnitOfWork.Terms.GetAllAsync(c => c.Id != termId.Value && c.TermType == ObjectType.category && !childIds.Contains(c.Id));
                }
                else
                {
                    terms = await UnitOfWork.Terms.GetAllAsync(c => c.Id != termId.Value && c.TermType == ObjectType.category);
                }
            }
            if (terms.Count < 1) return new DataResult<TermListDto>(ResultStatus.Error, Messages.NotFound(ObjectType.category, true), null);

            return new DataResult<TermListDto>(ResultStatus.Success, new TermListDto { Terms = terms });
        }

        public async Task<IDataResult<PostTermListDto>> GetAllPostTermAsync(int postId)
        {
            var postTerms = await UnitOfWork.PostTerms.GetAllAsync(pt => pt.PostId == postId, pt => pt.Term);
            if (postTerms.Count < 1) return new DataResult<PostTermListDto>(ResultStatus.Error, null);
            return new DataResult<PostTermListDto>(ResultStatus.Success, new PostTermListDto { PostTerms = postTerms });
        }

        public Task<IDataResult<TermListDto>> GetAllTermPageAsync(int termId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IDataResult<TermUpdateDto>> GetTermUpdateDtoAsync(int termId)
        {
            var term = await UnitOfWork.Terms.GetAsync(c => c.Id == termId, c => c.Parent, c => c.Children);
            if (term == null) return new DataResult<TermUpdateDto>(ResultStatus.Error, null);

            Term termParent = (Term)await ExtensionsHelper.GetParentsAsync(term.TermType, term);
            term.Parents = termParent.Parents;

            var termUpdateDto = Mapper.Map<TermUpdateDto>(term);
            return new DataResult<TermUpdateDto>(ResultStatus.Success, termUpdateDto);
        }

        public async Task<IDataResult<TermDto>> AddAsync(TermAddDto termAddDto)
        {
            string slug = string.IsNullOrWhiteSpace(termAddDto.Slug) ?  ExtensionsHelper.FriendlySEOString(termAddDto.Name) : termAddDto.Slug;
            var slugCheck = await UnitOfWork.Terms.GetAllAsync(t => t.Slug == slug && t.TermType == termAddDto.TermType);
            if (slugCheck.Count != 0) return new DataResult<TermDto>(ResultStatus.Error, Messages.UrlCheck(termAddDto.TermType), null);
            var term = Mapper.Map<Term>(termAddDto);

            if (termAddDto.Slug == null || termAddDto.Slug == "")
            {
                term.Slug = slug;
            }
            var addedTerm = await UnitOfWork.Terms.AddAsync(term);
            await UnitOfWork.SaveAsync();
            return new DataResult<TermDto>(ResultStatus.Success, Messages.Add(termAddDto.TermType, addedTerm.Name), new TermDto { Term = addedTerm });
        }

        public async Task<IDataResult<PostTermDto>> PostTermAddAsync(PostTermAddDto postTermAddDto)
        {
            var postTerm = Mapper.Map<PostTerm>(postTermAddDto);
            var addedPostTerm = await UnitOfWork.PostTerms.AddAsync(postTerm);
            await UnitOfWork.SaveAsync();
            return new DataResult<PostTermDto>(ResultStatus.Success, null, new PostTermDto { PostTerm = addedPostTerm });
        }

        public async Task<IDataResult<TermDto>> UpdateAsync(TermUpdateDto termUpdateDto)
        {
            var slugCheck = await UnitOfWork.Terms.GetAllAsync(t => (t.Slug == termUpdateDto.Slug && t.Id == termUpdateDto.Id) || (t.Slug != termUpdateDto.Slug && t.Id == termUpdateDto.Id));
            if (slugCheck.Count != 1) return new DataResult<TermDto>(ResultStatus.Error, Messages.UrlCheck(termUpdateDto.TermType), null);

            Term oldTerm = await UnitOfWork.Terms.GetAsync(p => p.Id == termUpdateDto.Id, p => p.Parent);
            var term = Mapper.Map<TermUpdateDto, Term>(termUpdateDto, oldTerm);

            var menuDetails = await UnitOfWork.MenuDetails.GetAllAsync(md => md.ObjectId == term.Id && md.ObjectType == term.TermType);

            if (menuDetails.Count > 0)
            {
                foreach (var menuDetail in menuDetails)
                {
                    menuDetail.CustomURL = await ExtensionsHelper.GetParentsURLAsync(term.TermType, term);
                    await UnitOfWork.MenuDetails.UpdateAsync(menuDetail);
                }
            }

            var updatedTerm = await UnitOfWork.Terms.UpdateAsync(term);
            await UnitOfWork.SaveAsync();
            return new DataResult<TermDto>(ResultStatus.Success, Messages.Update(termUpdateDto.TermType, term.Name), new TermDto { Term = term });
        }

        public async Task<IResult> PostTermDeleteAsync(int postId, int termId)
        {
            var postTerm = await UnitOfWork.PostTerms.GetAsync(x => x.PostId == postId && x.TermId == termId);
            if (postTerm == null) return new Result(ResultStatus.Error);
            await UnitOfWork.PostTerms.DeleteAsync(postTerm);
            await UnitOfWork.SaveAsync();
            return new Result(ResultStatus.Success);
        }

        public async Task<IResult> DeleteAsync(int termId)
        {
            var term = await UnitOfWork.Terms.GetAsync(t => t.Id == termId);
            if (term == null) return new Result(ResultStatus.Error, Messages.NotFound(term.TermType, false));

            var seo = await UnitOfWork.SeoObjectSettings.GetAsync(sos => sos.ObjectId == termId && sos.ObjectType == term.TermType);
            await UnitOfWork.Terms.DeleteAsync(term);
            await UnitOfWork.SeoObjectSettings.DeleteAsync(seo);
            await UnitOfWork.SaveAsync();
            return new Result(ResultStatus.Success, Messages.Delete(term.TermType, term.Name));
        }

        public async Task<IDataResult<int>> CountAsync()
        {
            var termsCount = await UnitOfWork.Terms.CountAsync();
            if (termsCount < 1) return new DataResult<int>(ResultStatus.Error, $"Beklenmeyen bir hata ile karşılaşıldı.", -1);
            return new DataResult<int>(ResultStatus.Success, termsCount);
        }
    }
}