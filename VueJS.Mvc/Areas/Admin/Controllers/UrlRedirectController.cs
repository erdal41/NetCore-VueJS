using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VueJS.Entities.Concrete;
using VueJS.Mvc.Areas.Admin.Models.Data;
using VueJS.Mvc.Areas.Admin.Models.View;
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
            return Json(new UrlRedirectViewModel { UrlRedirectListDto = await _urlRedirectService.GetAllAsync() });
        }

        [HttpPost("/admin/urlredirect-new")]
        public async Task<JsonResult> New(UrlRedirectDataModel urlRedirectDataModel)
        {
            return Json(new UrlRedirectViewModel { UrlRedirectDto = await _urlRedirectService.AddAsync(urlRedirectDataModel.UrlRedirectAddDto, LoggedInUser.Id) });
        }

        [HttpGet("/admin/urlredirect-edit")]
        public async Task<JsonResult> Edit(int urlRedirectId)
        {
            return Json(new UrlRedirectViewModel { UrlRedirectUpdateDto = await _urlRedirectService.GetUrlRedirectUpdateDtoAsync(urlRedirectId) });
        }

        [HttpPost("/admin/urlredirect-edit")]
        public async Task<JsonResult> Edit(UrlRedirectDataModel urlRedirectDataModel)
        {
            return Json(new UrlRedirectViewModel { UrlRedirectDto = await _urlRedirectService.UpdateAsync(urlRedirectDataModel.UrlRedirectUpdateDto, LoggedInUser.Id) });
        }

        [HttpPost("/admin/urlredirect-delete")]
        public async Task<JsonResult> Delete(int urlRedirectId)
        {
            return Json(await _urlRedirectService.DeleteAsync(urlRedirectId));
        }
    }
}