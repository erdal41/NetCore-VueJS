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

        [HttpGet("admin/upload-alluploadspartial")]
        public async Task<IActionResult> GetAllUploadsPartial()
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

        [HttpPost("admin/upload-new")]
        public async Task<IActionResult> New(IList<IFormFile> files)
        {
            foreach (IFormFile file in files)
            {
                var uploadAddDto = Mapper.Map<UploadAddDto>(file);
                var uploadResult = await ImageHelper.Upload(file);
                uploadAddDto.FileName = uploadResult.Data.FileFullName;
                uploadAddDto.AltText = uploadResult.Data.FileFullName;
                uploadAddDto.ContentType = uploadResult.Data.ContentType;
                uploadAddDto.Size = uploadResult.Data.Size;
                uploadAddDto.Width = uploadResult.Data.Width;
                uploadAddDto.Height = uploadResult.Data.Height;

                await _uploadService.AddAsync(uploadAddDto, LoggedInUser.Id);
            }
            return Json(files);
        }

        [HttpPost("admin/upload-postgalleryadd")]
        public async Task<JsonResult> PostGalleryAdd(GalleryAddDto galleryAddDto)
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

        [HttpGet("admin/upload-edit")]
        public async Task<IActionResult> Edit(int uploadId)
        {
            var result = await _uploadService.GetUploadUpdateDtoAsync(uploadId);

            if (result.ResultStatus == ResultStatus.Success)
            {
                return PartialView("_UploadUpdatePartial", result.Data);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("admin/upload-edit")]
        public async Task<IActionResult> Edit(UploadUpdateDto uploadUpdateDto)
        {
            var result = await _uploadService.UpdateAsync(uploadUpdateDto, LoggedInUser.Id);
            if (result.ResultStatus == ResultStatus.Success)
            {
                uploadUpdateDto.User = await UserManager.Users.FirstOrDefaultAsync(u => u.Id == LoggedInUser.Id);
                var uploadUpdateAjaxViewModel = JsonConvert.SerializeObject(await this.RenderViewToStringAsync("_UploadUpdatePartial", uploadUpdateDto));
                return Json(uploadUpdateAjaxViewModel);
            }
            var uploadUpdateAjaxErrorViewModel = JsonConvert.SerializeObject(await this.RenderViewToStringAsync("_UploadUpdatePartial", uploadUpdateDto));
            return Json(uploadUpdateAjaxErrorViewModel);
        }

        [HttpPost("admin/upload-delete")]
        public async Task<JsonResult> Delete(int uploadId)
        {
            var upload = await _uploadService.GetAsync(uploadId);
            var result = await _uploadService.DeleteAsync(uploadId);
            var uploadResult = JsonConvert.SerializeObject(result);
            if (result.ResultStatus == ResultStatus.Success)
            {
                ImageHelper.Delete(upload.Data.Upload.FileName);
                return Json(uploadResult);
            }
            return Json(uploadResult);
        }

        [HttpPost("admin/upload-galleryimagedelete")]
        public async Task<JsonResult> GalleryImageDelete(int postId, int uploadId)
        {
            var result = await _uploadService.GalleryImageDeleteAsync(postId, uploadId);
            var uploadResult = JsonConvert.SerializeObject(result);
            return Json(uploadResult);
        }        
    }
}