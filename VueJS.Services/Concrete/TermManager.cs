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
using VueJS.Services.Extensions;
using System.Collections.Generic;
using System.Linq;
using VueJS.Entities.ComplexTypes;

namespace VueJS.Services.Concrete
{
    public class TermManager : ManagerBase, ITermService
    {
        public TermManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper) { }

        public async Task<IDataResult<TermDto>> GetAsync(int termId)
        {
            var term = await UnitOfWork.Terms.GetAsync(t => t.Id == termId);
            if (term == null) return new DataResult<TermDto>(ResultStatus.Error, Messages.Term.NotFound(false), null);
            return new DataResult<TermDto>(ResultStatus.Success, new TermDto { Term = term });
        }

        public async Task<IDataResult<TermDto>> GetAsync(SubObjectType termType, string termSlug)
        {
            Term term = null;
            switch (termType)
            {
                case SubObjectType.category:
                    term = await UnitOfWork.Terms.GetAsync(c => c.TermType == SubObjectType.category && c.Slug == termSlug, c => c.Parent, c => c.Children);

                    Term parent = term.Parent;
                    term.Parents = new List<Term>();
                    while (parent != null)
                    {
                        term.Parents.Add(parent);
                        parent = await UnitOfWork.Terms.GetAsync(p => p.Id == parent.ParentId, p => p.Parent, p => p.Children);
                    }
                    break;
                case SubObjectType.tag:
                    term = await UnitOfWork.Terms.GetAsync(c => c.TermType == SubObjectType.tag && c.Slug == termSlug);
                    break;
            }

            if (term == null) return new DataResult<TermDto>(ResultStatus.Error, Messages.Term.NotFound(false), null);
            return new DataResult<TermDto>(ResultStatus.Success, new TermDto { Term = term });
        }

        public async Task<IDataResult<TermListDto>> GetAllAsync(SubObjectType termType)
        {
            var terms = await UnitOfWork.Terms.GetAllAsync(t => t.TermType == termType, t => t.Parent, t => t.Children, t => t.PostTerms);
            if (terms.Count < 1) return new DataResult<TermListDto>(ResultStatus.Error, Messages.Term.NotFound(true), null);

            foreach (var term in terms)
            {
                Term parent = term.Parent;
                term.Parents = new List<Term>();
                while (parent != null)
                {
                    term.Parents.Add(parent);
                    parent = await UnitOfWork.Terms.GetAsync(p => p.Id == parent.ParentId, p => p.Parent, p => p.Children);
                }
            }
            return new DataResult<TermListDto>(ResultStatus.Success, new TermListDto { Terms = terms });
        }

        public async Task<IDataResult<TermListDto>> GetAllParentAsync(int? termId)
        {
            IList<Term> terms = null;
            if (termId == null)
            {
                terms = await UnitOfWork.Terms.GetAllAsync(t => t.TermType == SubObjectType.category);
            }
            else
            {
                var currentTerm = await UnitOfWork.Terms.GetAsync(t => t.Id == termId.Value, c => c.Children, c => c.Parent, c => c.Parents);

                if (currentTerm != null)
                {
                    Term parent = currentTerm.Parent;
                    currentTerm.Parents = new List<Term>();
                    while (parent != null)
                    {
                        currentTerm.Parents.Add(parent);
                        parent = await UnitOfWork.Terms.GetAsync(p => p.Id == parent.ParentId, p => p.Parent, p => p.Children);
                    }

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
                    terms = await UnitOfWork.Terms.GetAllAsync(c => c.Id != termId.Value && c.TermType == SubObjectType.category && !childIds.Contains(c.Id));
                }
                else
                {
                    terms = await UnitOfWork.Terms.GetAllAsync(c => c.Id != termId.Value && c.TermType == SubObjectType.category);
                }
            }
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
            var result = await UnitOfWork.Terms.AnyAsync(c => c.Id == termId);
            if (!result) return new DataResult<TermUpdateDto>(ResultStatus.Error, Messages.Term.NotFound(false), null);

            var term = await UnitOfWork.Terms.GetAsync(c => c.Id == termId, c => c.Parent, c => c.Children);
            Term parent = term.Parent;
            term.Parents = new List<Term>();
            while (parent != null)
            {
                term.Parents.Add(parent);
                parent = await UnitOfWork.Terms.GetAsync(p => p.Id == parent.ParentId, p => p.Parent, p => p.Children);
            }
            var termUpdateDto = Mapper.Map<TermUpdateDto>(term);
            return new DataResult<TermUpdateDto>(ResultStatus.Success, termUpdateDto);
        }

        public async Task<IDataResult<TermDto>> AddAsync(TermAddDto termAddDto)
        {
            string slug = UrlExtensions.FriendlySEOUrl(termAddDto.Name);
            var slugCheck = await UnitOfWork.Terms.GetAllAsync(t => t.Slug == slug && t.TermType == termAddDto.TermType);
            if (slugCheck.Count != 0) return new DataResult<TermDto>(ResultStatus.Error, termAddDto.TermType == SubObjectType.category ? Messages.Category.UrlCheck() : Messages.Tag.UrlCheck(), null);
            var term = Mapper.Map<Term>(termAddDto);

            if (termAddDto.Slug == null || termAddDto.Slug == "")
            {
                term.Slug = slug;
            }
            var addedTerm = await UnitOfWork.Terms.AddAsync(term);
            await UnitOfWork.SaveAsync();
            return new DataResult<TermDto>(ResultStatus.Success, termAddDto.TermType == SubObjectType.category ? Messages.Category.Add(addedTerm.Name) : Messages.Tag.Add(addedTerm.Name), new TermDto { Term = addedTerm });
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
            if (slugCheck.Count != 1) return new DataResult<TermDto>(ResultStatus.Error, termUpdateDto.TermType == SubObjectType.category ? Messages.Category.UrlCheck() : Messages.Tag.UrlCheck(), null);

            Term oldTerm = null;
            switch (slugCheck.FirstOrDefault().TermType)
            {
                case SubObjectType.category:
                    oldTerm = await UnitOfWork.Terms.GetAsync(p => p.Id == termUpdateDto.Id, p => p.Parent);
                    break;
                case SubObjectType.tag:
                    oldTerm = await UnitOfWork.Terms.GetAsync(p => p.Id == termUpdateDto.Id);
                    break;
            }
            var term = Mapper.Map<TermUpdateDto, Term>(termUpdateDto, oldTerm);
            var updatedTerm = await UnitOfWork.Terms.UpdateAsync(term);
            await UnitOfWork.SaveAsync();
            return new DataResult<TermDto>(ResultStatus.Success, termUpdateDto.TermType == SubObjectType.category ? Messages.Category.Update(term.Name) : Messages.Tag.Update(term.Name), new TermDto { Term = term });
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
            if (term == null) return new Result(ResultStatus.Error, term.TermType == SubObjectType.category ? Messages.Category.NotFound(false) : Messages.Tag.NotFound(false));

            var seo = await UnitOfWork.SeoObjectSettings.GetAsync(sos => sos.ObjectId == termId && sos.SubObjectType == term.TermType);
            await UnitOfWork.Terms.DeleteAsync(term);
            await UnitOfWork.SeoObjectSettings.DeleteAsync(seo);
            await UnitOfWork.SaveAsync();
            return new Result(ResultStatus.Success, term.TermType == SubObjectType.category ? Messages.Category.Delete(term.Name) : Messages.Tag.Delete(term.Name));
        }

        public async Task<IDataResult<int>> CountAsync()
        {
            var termsCount = await UnitOfWork.Terms.CountAsync();
            if (termsCount < 1) return new DataResult<int>(ResultStatus.Error, $"Beklenmeyen bir hata ile karşılaşıldı.", -1);
            return new DataResult<int>(ResultStatus.Success, termsCount);
        }
    }
}