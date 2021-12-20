using VueJS.Entities.Dtos;
using VueJS.Shared.Utilities.Results.Abstract;
using System.Threading.Tasks;

namespace VueJS.Services.Abstract
{
    public interface IUrlRedirectService
    {
        Task<IDataResult<UrlRedirectDto>> GetAsync(int urlRedirectId);
        Task<IDataResult<UrlRedirectDto>> GetAsync(string oldUrl);
        Task<IDataResult<UrlRedirectUpdateDto>> GetUrlRedirectUpdateDtoAsync(int urlRedirectId);
        Task<IDataResult<UrlRedirectListDto>> GetAllAsync();        
        Task<IDataResult<UrlRedirectDto>> AddAsync(UrlRedirectAddDto urlRedirectAddDto, int userId);
        Task<IDataResult<UrlRedirectDto>> UpdateAsync(UrlRedirectUpdateDto urlRedirectUpdateDto, int userId);
        Task<IResult> DeleteAsync(int urlRedirectId);
    }
}