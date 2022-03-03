using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VueJS.Entities.Concrete;
using VueJS.Mvc.Areas.Admin.Models;
using VueJS.Services.Abstract;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace VueJS.Mvc.Areas.Admin.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UrlRedirectController : BaseController
    {
        private readonly IUrlRedirectService _urlRedirectService;

        public UrlRedirectController(IUrlRedirectService urlRedirectService, UserManager<User> userManager, IMapper mapper) : base(userManager, mapper)
        {
            _urlRedirectService = urlRedirectService;
        }

        [HttpGet("/admin/urlredirect-allurlredirects")]
        public async Task<JsonResult> AllUrlRedirects()
        {
            return Json(await _urlRedirectService.GetAllAsync());
        }

        [HttpPost("/admin/urlredirect-new")]
        public async Task<JsonResult> New(UrlRedirectViewModel urlRedirectViewModel)
        {
            var result = await _urlRedirectService.AddAsync(urlRedirectViewModel.UrlRedirectAddDto, LoggedInUser.Id);
            var urlRedirectViewModelJson = new UrlRedirectViewModel
            {
                UrlRedirectDto = result.Data,
            };
            return Json(urlRedirectViewModelJson);
        }

        [HttpGet("/admin/urlredirect-edit")]
        public async Task<JsonResult> Edit(int urlRedirectId)
        {
            return Json(await _urlRedirectService.GetUrlRedirectUpdateDtoAsync(urlRedirectId));
        }

        [HttpPost("/admin/urlredirect-edit")]
        public async Task<JsonResult> Edit(UrlRedirectViewModel urlRedirectViewModel)
        {
            var result = await _urlRedirectService.UpdateAsync(urlRedirectViewModel.UrlRedirectUpdateDto, LoggedInUser.Id);
            var urlRedirectViewModelJson = new UrlRedirectViewModel
            {
                UrlRedirectDto = result.Data
            };
            return Json(urlRedirectViewModelJson);
        }

        [HttpPost("/admin/urlredirect-delete")]
        public async Task<JsonResult> Delete(int urlRedirectId)
        {
            return Json(await _urlRedirectService.DeleteAsync(urlRedirectId));
        }
    }
}