using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VueJS.Entities.Concrete;
using VueJS.Entities.Dtos;
using VueJS.Mvc.Areas.Admin.Models;
using VueJS.Mvc.Helpers.Abstract;
using VueJS.Services.Abstract;
using VueJS.Shared.Utilities.Extensions;
using VueJS.Shared.Utilities.Results.ComplexTypes;
using Newtonsoft.Json;
using System.Threading.Tasks;
using VueJS.Mvc.Areas.Admin.Controllers;

namespace VueJS.Mvc.Areas.Admin.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UrlRedirectController : BaseController
    {
        private readonly IUrlRedirectService _urlRedirectService;

        public UrlRedirectController(IUrlRedirectService urlRedirectService, UserManager<User> userManager, IMapper mapper, IImageHelper imageHelper) : base(userManager, mapper, imageHelper)
        {
            _urlRedirectService = urlRedirectService;
        }

        [Authorize(Roles = "SuperAdmin,UrlRedirect.Read")]
        [HttpGet]
        [Route("admin/urlredirect")]
        public async Task<IActionResult> Index()
        {
            var result = await _urlRedirectService.GetAllAsync();
            if (result.ResultStatus == ResultStatus.Success) return View(result.Data);
            return NotFound();
        }

        [HttpGet]
        [Route("admin/urlredirect/allurlredirects")]
        public async Task<JsonResult> AllUrlRedirects()
        {
            var result = await _urlRedirectService.GetAllAsync();
            return Json(result.Data);
        }

        [Authorize(Roles = "SuperAdmin,UrlRedirect.Create")]
        [HttpPost]
        [Route("admin/url-redirect/new")]
        public async Task<IActionResult> New(UrlRedirectAddDto urlRedirectAddDto)
        {
            //if (!urlRedirectAddDto.OldUrl.Contains("http://") || !urlRedirectAddDto.OldUrl.Contains("http://"))
            //{
            //    var urlRedirectAddAjaxModel = JsonConvert.SerializeObject(new UrlRedirectViewModel
            //    {
            //        UrlRedirectDto = result.Data,
            //    });
            //    return Json(urlRedirectAddAjaxModel);
            //}

            var result = await _urlRedirectService.AddAsync(urlRedirectAddDto, LoggedInUser.Id);
            if (result.ResultStatus == ResultStatus.Success)
            {
                var urlRedirectAddAjaxModel = JsonConvert.SerializeObject(new UrlRedirectViewModel
                {
                    UrlRedirectDto = result.Data,
                });
                return Json(urlRedirectAddAjaxModel);
            }
            else
            {
                var urlRedirectAddAjaxErrorModel = JsonConvert.SerializeObject(new UrlRedirectViewModel
                {
                    UrlRedirectDto = null
                });
                return Json(urlRedirectAddAjaxErrorModel);
            }
        }

        [Authorize(Roles = "SuperAdmin,UrlRedirect.Update")]
        [HttpGet]
        [Route("admin/url-redirect/edit")]
        public async Task<IActionResult> Edit(int urlRedirectId)
        {
            var result = await _urlRedirectService.GetUrlRedirectUpdateDtoAsync(urlRedirectId);
            if (result.ResultStatus == ResultStatus.Success)
            {
                return PartialView("_UrlRedirectUpdatePartial", result.Data);
            }
            else
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "SuperAdmin,UrlRedirect.Update")]
        [HttpPost]
        [Route("admin/url-redirect/edit")]
        public async Task<IActionResult> Edit(UrlRedirectUpdateDto urlRedirectUpdateDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _urlRedirectService.UpdateAsync(urlRedirectUpdateDto, LoggedInUser.Id);
                if (result.ResultStatus == ResultStatus.Success)
                {
                    string urlRedirectAjaxViewModel = JsonConvert.SerializeObject(new UrlRedirectViewModel
                    {
                        UrlRedirectDto = result.Data,
                        UrlRedirectUpdatePartial = await this.RenderViewToStringAsync("_UrlRedirectUpdatePartial", urlRedirectUpdateDto)
                    }, new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                    return Json(urlRedirectAjaxViewModel);
                }
            }
            var urlRedirectAjaxErrorViewModel = JsonConvert.SerializeObject(new UrlRedirectViewModel
            {
                UrlRedirectUpdatePartial = await this.RenderViewToStringAsync("_UrlRedirectUpdatePartial", urlRedirectUpdateDto)
            });
            return Json(urlRedirectAjaxErrorViewModel);
        }

        [Authorize(Roles = "SuperAdmin,UrlRedirect.Delete")]
        [HttpPost]
        [Route("admin/url-redirect/delete")]
        public async Task<JsonResult> Delete(int urlRedirectId)
        {
            var result = await _urlRedirectService.DeleteAsync(urlRedirectId);
            var deletedurlRedirect = JsonConvert.SerializeObject(result);
            return Json(deletedurlRedirect);
        }

        [Authorize(Roles = "SuperAdmin,UrlRedirect.Delete")]
        [HttpPost]
        [Route("admin/url-redirect/multidelete")]
        public async Task<JsonResult> MultiDelete(int[] urlRedirectIds)
        {
            var jsonResult = string.Empty;
            foreach (var urlRedirectId in urlRedirectIds)
            {
                var result = await _urlRedirectService.DeleteAsync(urlRedirectId);
                var deletedurlRedirect = JsonConvert.SerializeObject(result);
                jsonResult = deletedurlRedirect;
            }
            return Json(jsonResult);
        }
    }
}