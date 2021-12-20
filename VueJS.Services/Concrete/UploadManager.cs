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

namespace VueJS.Services.Concrete
{
    public class UploadManager : ManagerBase, IUploadService
    {
        public UploadManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public async Task<IDataResult<UploadDto>> GetAsync(int uploadId)
        {
            var upload = await UnitOfWork.Uploads.GetAsync(u => u.Id == uploadId, u => u.User);
            if (upload != null)
            {
                return new DataResult<UploadDto>(ResultStatus.Success, new UploadDto
                {
                    Upload = upload,
                    ResultStatus = ResultStatus.Success
                });
            }
            return new DataResult<UploadDto>(ResultStatus.Error, Messages.Upload.NotFound(isPlural: false), null);
        }

        public async Task<IDataResult<UploadListDto>> GetAllAsync()
        {
            var uploads = await UnitOfWork.Uploads.GetAllAsync(null, u => u.User);
            if (uploads.Count > -1)
            {
                return new DataResult<UploadListDto>(ResultStatus.Success, new UploadListDto
                {
                    Uploads = uploads,
                    ResultStatus = ResultStatus.Success
                });
            }
            return new DataResult<UploadListDto>(ResultStatus.Error, Messages.Upload.NotFound(isPlural: true), new UploadListDto
            {
                Uploads = null,
                ResultStatus = ResultStatus.Error,
                Message = Messages.Upload.NotFound(isPlural: true)
            });
        }

        public async Task<IDataResult<UploadUpdateDto>> GetUploadUpdateDtoAsync(int uploadId)
        {
            var result = await UnitOfWork.Uploads.AnyAsync(u => u.Id == uploadId);
            if (result)
            {
                var upload = await UnitOfWork.Uploads.GetAsync(u => u.Id == uploadId, u => u.User);
                var uploadUpdateDto = Mapper.Map<UploadUpdateDto>(upload);
                return new DataResult<UploadUpdateDto>(ResultStatus.Success, uploadUpdateDto);
            }
            else
            {
                return new DataResult<UploadUpdateDto>(ResultStatus.Error, Messages.Upload.NotFound(isPlural: false), null);
            }
        }

        public async Task<IDataResult<UploadDto>> AddAsync(UploadAddDto uploadAddDto, int userId)
        {
            var upload = Mapper.Map<Upload>(uploadAddDto);
            upload.FileUrl = "/assets/ap/img/" + uploadAddDto.FileName;
            upload.UserId = userId;
            var addedUpload = await UnitOfWork.Uploads.AddAsync(upload);
            await UnitOfWork.SaveAsync();
            return new DataResult<UploadDto>(ResultStatus.Success, Messages.Upload.Add(addedUpload.FileName), new UploadDto
            {
                Upload = addedUpload,
                ResultStatus = ResultStatus.Success,
                Message = Messages.Upload.Add(addedUpload.FileName)
            });
        }

        public async Task<IResult> UpdateAsync(UploadUpdateDto uploadUpdateDto, int userId)
        {
            var oldUpload = await UnitOfWork.Uploads.GetAsync(u => u.Id == uploadUpdateDto.Id, u => u.User);
            var upload = Mapper.Map<UploadUpdateDto, Upload>(uploadUpdateDto, oldUpload);
            upload.UserId = userId;
            var updateUpload = await UnitOfWork.Uploads.UpdateAsync(upload);
            await UnitOfWork.SaveAsync();
            return new Result(ResultStatus.Success, Messages.Upload.Update(upload.FileName));
        }

        public async Task<IResult> DeleteAsync(int uploadId)
        {
            var result = await UnitOfWork.Uploads.AnyAsync(u => u.Id == uploadId);
            if (result)
            {
                var upload = await UnitOfWork.Uploads.GetAsync(u => u.Id == uploadId);
                await UnitOfWork.Uploads.DeleteAsync(upload);
                await UnitOfWork.SaveAsync();
                return new Result(ResultStatus.Success, Messages.Upload.Delete(upload.FileName));
            }
            return new Result(ResultStatus.Error, Messages.Upload.NotFound(isPlural: false));
        }

        public async Task<IResult> GalleryImageDeleteAsync(int postId, int uploadId)
        {
            var gallery = await UnitOfWork.Galleries.GetAsync(g => g.PostId == postId && g.UploadId == uploadId);

            if (gallery != null)
            {
                await UnitOfWork.Galleries.DeleteAsync(gallery);
                await UnitOfWork.SaveAsync();
                return new Result(ResultStatus.Success, "Galeri güncellendi.");
            }
            return new Result(ResultStatus.Error, "Hata. Sayfayı yenileyip tekrar deneyiniz.");
        }

        public async Task<IDataResult<GalleryDto>> GalleryAddAsync(GalleryAddDto galleryAddDto)
        {
            var result = await UnitOfWork.Galleries.AnyAsync(g => g.PostId == galleryAddDto.PostId && g.UploadId == galleryAddDto.UploadId);
            if (result) return new DataResult<GalleryDto>(ResultStatus.Error, null);

            var gallery = Mapper.Map<Gallery>(galleryAddDto);
            await UnitOfWork.Galleries.AddAsync(gallery);
            await UnitOfWork.SaveAsync();
            return new DataResult<GalleryDto>(ResultStatus.Success, null, new GalleryDto
            {
                Gallery = gallery,
                ResultStatus = ResultStatus.Success,
            });
        }
    }
}