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

        [HttpGet("/admin/urlredirect/allurlredirects")]
        public async Task<JsonResult> AllUrlRedirects()
        {
            var result = await _urlRedirectService.GetAllAsync();
            return Json(result.Data);
        }

        [HttpPost("/admin/urlredirect/new")]
        public async Task<JsonResult> New(UrlRedirectViewModel urlRedirectViewModel)
        {
            var result = await _urlRedirectService.AddAsync(urlRedirectViewModel.UrlRedirectAddDto, 1);
            var urlRedirectViewModelJson = new UrlRedirectViewModel
            {
                UrlRedirectDto = result.Data,
            };
            return Json(urlRedirectViewModelJson);
        }

        [HttpGet("/admin/urlredirect/edit")]
        public async Task<JsonResult> Edit(int urlRedirectId)
        {
            var result = await _urlRedirectService.GetUrlRedirectUpdateDtoAsync(urlRedirectId);
            return Json(result.Data);
        }

        [HttpPost("/admin/urlredirect/edit")]
        public async Task<JsonResult> Edit(UrlRedirectViewModel urlRedirectViewModel)
        {
            var result = await _urlRedirectService.UpdateAsync(urlRedirectViewModel.UrlRedirectUpdateDto, 1);
            var urlRedirectViewModelJson = new UrlRedirectViewModel
            {
                UrlRedirectDto = result.Data
            };
            return Json(urlRedirectViewModelJson);
        }

        [HttpPost("/admin/urlredirect/delete")]
        public async Task<JsonResult> Delete(int urlRedirectId)
        {
            var result = await _urlRedirectService.DeleteAsync(urlRedirectId);
            return Json(result);
        }
    }
}