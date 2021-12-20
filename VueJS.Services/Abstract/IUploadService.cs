using VueJS.Entities.Dtos;
using VueJS.Shared.Utilities.Results.Abstract;
using System.Threading.Tasks;

namespace VueJS.Services.Abstract
{
    public interface IUploadService
    {
        Task<IDataResult<UploadDto>> GetAsync(int uploadId);
        Task<IDataResult<UploadListDto>> GetAllAsync();
        Task<IDataResult<UploadUpdateDto>> GetUploadUpdateDtoAsync(int uploadId);
        Task<IDataResult<UploadDto>> AddAsync(UploadAddDto uploadAddDto, int userId);
        Task<IDataResult<GalleryDto>> GalleryAddAsync(GalleryAddDto galleryAddDto);
        Task<IResult> UpdateAsync(UploadUpdateDto uploadUpdateDto, int userId);
        Task<IResult> DeleteAsync(int uploadId);
        Task<IResult> GalleryImageDeleteAsync(int postId, int uploadId);
    }
}