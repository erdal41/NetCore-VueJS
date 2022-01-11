using VueJS.Entities.ComplexTypes;
using VueJS.Entities.Dtos;
using VueJS.Shared.Utilities.Results.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VueJS.Services.Abstract
{
    public interface ITermService
    {
        Task<IDataResult<TermDto>> GetAsync(int termId);
        Task<IDataResult<TermDto>> GetAsync(SubObjectType termtype, string termSlug);
        Task<IDataResult<TermListDto>> GetAllAsync(SubObjectType termType);
        Task<IDataResult<TermListDto>> GetAllParentAsync(int? termId);
        Task<IDataResult<TermListDto>> GetAllTermPageAsync(int termId);
        Task<IDataResult<TermUpdateDto>> GetTermUpdateDtoAsync(int termId);
        Task<IDataResult<PostTermListDto>> GetAllPostTermAsync(int postId);
        Task<IDataResult<TermDto>> AddAsync(TermAddDto termAddDto);
        Task<IDataResult<PostTermDto>> PostTermAddAsync(PostTermAddDto postTermsAddDto);
        Task<IDataResult<TermDto>> UpdateAsync(TermUpdateDto termUpdateDto);
        Task<IResult> PostTermDeleteAsync(int postId, int termId);
        Task<IResult> DeleteAsync(int termId);
        Task<IDataResult<int>> CountAsync();
    }
}