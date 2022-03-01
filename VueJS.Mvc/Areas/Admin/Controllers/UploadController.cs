using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VueJS.Mvc.Areas.Admin.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using VueJS.Mvc.Areas.Admin.Controllers;
using VueJS.Services.Abstract;
using VueJS.Entities.Concrete;
using VueJS.Mvc.Helpers.Abstract;
using VueJS.Shared.Utilities.Results.ComplexTypes;
using VueJS.Entities.Dtos;
using VueJS.Shared.Utilities.Extensions;

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

        [HttpGet("/admin/upload-alluploadspartial")]
        public async Task<ActionResult> GetAllUploadsPartial()
        {
            var result = await _uploadService.GetAllAsync();
            if (result.ResultStatus == ResultStatus.Success)
            {
                return PartialView("_GetAllUploadsPartial", result.Data);
            }
            else
            {
                return NotFound();
            }
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

                    uploadViewModel = new UploadViewModel
                    {
                        UploadDtos = uploadDtos
                    };
                }
                else
                {
                    uploadViewModel = new UploadViewModel
                    {
                        UploadDtos = null
                    };
                }
            }
            return Json(uploadViewModel);
        }

        [HttpPost("/admin/upload-postgalleryadd")]
        public async Task<ActionResult> PostGalleryAdd(GalleryAddDto galleryAddDto)
        {
            var result = await _uploadService.GalleryAddAsync(galleryAddDto);
            if (result.ResultStatus == ResultStatus.Success)
            {
                var galleryAddAjaxModel = JsonConvert.SerializeObject(new GalleryViewModel
                {
                    GalleryDto = result.Data,
                });
                return Json(galleryAddAjaxModel);
            }
            else
            {
                var galleryAddAjaxErrorViewModel = JsonConvert.SerializeObject(new GalleryViewModel
                {
                    GalleryDto = result.Data,
                });
                return Json(galleryAddAjaxErrorViewModel);
            }
        }

        [HttpGet("/admin/upload-edit")]
        public async Task<JsonResult> Edit(int uploadId)
        {
            var result = await _uploadService.GetUploadUpdateDtoAsync(uploadId);

            if (result.ResultStatus == ResultStatus.Success)
            {
                return Json(new UploadViewModel
                {
                    UploadUpdateDto = result.Data
                });
            }
            else
            {
                return Json(new UploadViewModel
                {
                    UploadUpdateDto = null
                });
            }
        }

        [HttpPost("/admin/upload-edit")]
        public async Task<JsonResult> Edit(UploadUpdateDto uploadUpdateDto)
        {
            var result = await _uploadService.UpdateAsync(uploadUpdateDto, LoggedInUser.Id);
            if (result.ResultStatus == ResultStatus.Success)
            {
                uploadUpdateDto.User = await UserManager.Users.FirstOrDefaultAsync(u => u.Id == LoggedInUser.Id);
                return Json(new UploadViewModel
                {
                    UploadDto = result.Data
                });
            }
            else
            {
                return Json(new UploadViewModel
                {
                    UploadDto = null
                });
            }
        }

        [HttpPost("/admin/upload-delete")]
        public async Task<JsonResult> Delete(int uploadId)
        {
            var upload = await _uploadService.GetAsync(uploadId);
            var result = await _uploadService.DeleteAsync(uploadId);
            if (result.ResultStatus == ResultStatus.Success)
            {
                ImageHelper.Delete(upload.Data.Upload.FileName);
                return Json(result);
            }
            return Json(result);
        }

        [HttpPost("/admin/upload-galleryimagedelete")]
        public async Task<JsonResult> GalleryImageDelete(int postId, int uploadId)
        {
            var result = await _uploadService.GalleryImageDeleteAsync(postId, uploadId);
            var uploadResult = JsonConvert.SerializeObject(result);
            return Json(uploadResult);
        }
    }
}