using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VueJS.Mvc.Areas.Admin.Models.Data;
using VueJS.Mvc.Areas.Admin.Models.View;
using System.Collections.Generic;
using System.Threading.Tasks;
using VueJS.Services.Abstract;
using VueJS.Entities.Concrete;
using VueJS.Mvc.Helpers.Abstract;
using VueJS.Shared.Utilities.Results.ComplexTypes;
using VueJS.Entities.Dtos;

namespace VueJS.Mvc.Areas.Admin.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UploadController : BaseController
    {
        private readonly IUploadService _uploadService;

        public UploadController(IUploadService uploadService, UserManager<User> userManager, IMapper mapper, IImageHelper imageHelper) : base(userManager, mapper, imageHelper)
        {
            _uploadService = uploadService;
        }

        [HttpGet("/admin/upload-alluploads")]
        public async Task<JsonResult> GetAll()
        {
            var uploads = await _uploadService.GetAllAsync();
            return new JsonResult(uploads);
        }

        [HttpPost("/admin/upload-new")]
        public async Task<JsonResult> New(IList<IFormFile> files)
        {
            var uploadDtos = new List<UploadDto>();
            var uploadViewModel = new UploadViewModel();
            foreach (IFormFile file in files)
            {
                var uploadResult = await ImageHelper.Upload(file, LoggedInUser.Id);

                if (uploadResult.ResultStatus == ResultStatus.Success)
                {
                    uploadDtos.Add(uploadResult.Data);
                    uploadViewModel = new UploadViewModel { UploadDtos = uploadDtos };
                }
                else
                {
                    uploadViewModel = new UploadViewModel { UploadDtos = null };
                }
            }
            return Json(uploadViewModel);
        }

        [HttpPost("/admin/upload-postgalleryadd")]
        public async Task<ActionResult> PostGalleryAdd(UploadDataModel uploadDataModel)
        {
            return Json(new UploadViewModel { GalleryDto = await _uploadService.GalleryAddAsync(uploadDataModel.GalleryAddDto) });
        }

        [HttpGet("/admin/upload-edit")]
        public async Task<JsonResult> Edit(int uploadId)
        {
            return Json(new UploadViewModel { UploadUpdateDto = await _uploadService.GetUploadUpdateDtoAsync(uploadId) });
        }

        [HttpPost("/admin/upload-edit")]
        public async Task<JsonResult> Edit(UploadDataModel uploadDataModel)
        {
            var result = await _uploadService.UpdateAsync(uploadDataModel.UploadUpdateDto, LoggedInUser.Id);
            if (result.ResultStatus == ResultStatus.Success)
                uploadDataModel.UploadUpdateDto.User = await UserManager.Users.FirstOrDefaultAsync(u => u.Id == LoggedInUser.Id);
            return Json(new UploadViewModel { UploadDto = result });
        }

        [HttpPost("/admin/upload-delete")]
        public async Task<JsonResult> Delete(int uploadId)
        {
            var result = await _uploadService.DeleteAsync(uploadId);
            if (result.ResultStatus == ResultStatus.Success)
                await ImageHelper.Delete(uploadId);
            return Json(result);
        }

        [HttpPost("/admin/upload-galleryimagedelete")]
        public async Task<JsonResult> GalleryImageDelete(int postId, int uploadId)
        {
            return Json(await _uploadService.GalleryImageDeleteAsync(postId, uploadId));
        }
    }
}