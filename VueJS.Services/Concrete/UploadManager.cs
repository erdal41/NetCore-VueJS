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
using System.Linq;

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
            if (upload == null) return new DataResult<UploadDto>(ResultStatus.Error, Messages.Upload.NotFound(false), null);
            return new DataResult<UploadDto>(ResultStatus.Success, new UploadDto { Upload = upload });
        }

        public async Task<IDataResult<UploadListDto>> GetAllAsync()
        {
            var uploads = await UnitOfWork.Uploads.GetAllAsync(null, u => u.User);
            if (uploads.Count < 0) return new DataResult<UploadListDto>(ResultStatus.Error, Messages.Upload.NotFound(true), null);
            return new DataResult<UploadListDto>(ResultStatus.Success, new UploadListDto { Uploads = uploads.OrderByDescending(x => x.CreatedDate).ToList() });
        }

        public async Task<IDataResult<UploadUpdateDto>> GetUploadUpdateDtoAsync(int uploadId)
        {
            var result = await UnitOfWork.Uploads.AnyAsync(u => u.Id == uploadId);
            if (!result) return new DataResult<UploadUpdateDto>(ResultStatus.Error, Messages.Upload.NotFound(false), null);
            var upload = await UnitOfWork.Uploads.GetAsync(u => u.Id == uploadId, u => u.User);
            var uploadUpdateDto = Mapper.Map<UploadUpdateDto>(upload);
            return new DataResult<UploadUpdateDto>(ResultStatus.Success, uploadUpdateDto);
        }

        public async Task<IDataResult<UploadDto>> AddAsync(UploadAddDto uploadAddDto, int userId)
        {
            var upload = Mapper.Map<Upload>(uploadAddDto);
            upload.UserId = userId;
            var addedUpload = await UnitOfWork.Uploads.AddAsync(upload);
            await UnitOfWork.SaveAsync();
            return new DataResult<UploadDto>(ResultStatus.Success, Messages.Upload.Add(addedUpload.FileName), new UploadDto { Upload = addedUpload });
        }

        public async Task<IDataResult<UploadDto>> UpdateAsync(UploadUpdateDto uploadUpdateDto, int userId)
        {
            var oldUpload = await UnitOfWork.Uploads.GetAsync(u => u.Id == uploadUpdateDto.Id, u => u.User);
            if (oldUpload == null) return new DataResult<UploadDto>(ResultStatus.Error, Messages.Upload.NotFound(false), null);

            var upload = Mapper.Map<UploadUpdateDto, Upload>(uploadUpdateDto, oldUpload);
            upload.UserId = userId;
            var updateUpload = await UnitOfWork.Uploads.UpdateAsync(upload);
            await UnitOfWork.SaveAsync();
            return new DataResult<UploadDto>(ResultStatus.Success, Messages.Upload.Update(upload.FileName), new UploadDto { Upload = updateUpload });
        }

        public async Task<IResult> DeleteAsync(int uploadId)
        {
            var upload = await UnitOfWork.Uploads.GetAsync(u => u.Id == uploadId);
            if (upload == null) return new Result(ResultStatus.Error, Messages.Upload.NotFound(false));
            await UnitOfWork.Uploads.DeleteAsync(upload);
            await UnitOfWork.SaveAsync();
            return new Result(ResultStatus.Success, Messages.Upload.Delete(upload.FileName));
        }

        public async Task<IResult> GalleryImageDeleteAsync(int postId, int uploadId)
        {
            var gallery = await UnitOfWork.Galleries.GetAsync(g => g.PostId == postId && g.UploadId == uploadId);
            if (gallery == null) return new Result(ResultStatus.Error, "Hata. Sayfayı yenileyip tekrar deneyiniz.");
            await UnitOfWork.Galleries.DeleteAsync(gallery);
            await UnitOfWork.SaveAsync();
            return new Result(ResultStatus.Success, "Seçilen resim galeriden silindi.");
        }

        public async Task<IDataResult<GalleryDto>> GalleryAddAsync(GalleryAddDto galleryAddDto)
        {
            var gallery = Mapper.Map<Gallery>(galleryAddDto);
            await UnitOfWork.Galleries.AddAsync(gallery);
            await UnitOfWork.SaveAsync();
            return new DataResult<GalleryDto>(ResultStatus.Success, "Seçilen resim galeriye eklendi.", new GalleryDto { Gallery = gallery });
        }
    }
}