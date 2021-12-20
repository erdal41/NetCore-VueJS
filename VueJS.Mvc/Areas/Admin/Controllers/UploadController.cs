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

        [Authorize(Roles = "SuperAdmin,Article.Read")]
        [HttpGet]
        [Route("admin/upload")]
        public async Task<IActionResult> Index()
        {
            var result = await _uploadService.GetAllAsync();
            if (result.ResultStatus == ResultStatus.Success) return View(result.Data);
            return NotFound();
        }

        [HttpGet]
        [Route("/admin/uploads/getall")]
        public async Task<JsonResult> GetAll()
         {
            var uploads = await _uploadService.GetAllAsync();            
            return new JsonResult(uploads);
        }

        [HttpGet]
        [Route("admin/upload/getalluploadspartial")]
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

        [Authorize(Roles = "SuperAdmin,Article.Create")]
        [HttpPost]
        [Route("admin/upload/new")]
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

        [Route("admin/upload/postgalleryadd")]
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

        [Authorize(Roles = "SuperAdmin,Article.Update")]
        [HttpGet]
        [Route("admin/upload/edit")]
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

        [Authorize(Roles = "SuperAdmin,Article.Update")]
        [HttpPost]
        [Route("admin/upload/edit")]
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

        [Authorize(Roles = "SuperAdmin,Article.Update")]
        [HttpPost]
        [Route("admin/upload/delete")]
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

        [Authorize(Roles = "SuperAdmin,Article.Update")]
        [HttpPost]
        [Route("admin/upload/galleryimagedelete")]
        public async Task<JsonResult> GalleryImageDelete(int postId, int uploadId)
        {
            var result = await _uploadService.GalleryImageDeleteAsync(postId, uploadId);
            var uploadResult = JsonConvert.SerializeObject(result);
            return Json(uploadResult);
        }

        [Authorize(Roles = "SuperAdmin,Article.Update")]
        [HttpPost]
        [Route("admin/upload/multidelete")]
        public async Task<JsonResult> MultiDelete(int[] uploadIds)
        {
            var jsonResult = string.Empty;
            foreach (var uploadId in uploadIds)
            {
                var result = await _uploadService.DeleteAsync(uploadId);
                var upload = await _uploadService.GetAsync(uploadId);
                ImageHelper.Delete(upload.Data.Upload.FileName);
                var uploadResult = JsonConvert.SerializeObject(result);
                jsonResult = uploadResult;
            }
            return Json(jsonResult);
        }
    }
}