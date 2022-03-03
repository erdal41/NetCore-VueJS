using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AutoMapper;
using VueJS.Services.Abstract;
using VueJS.Entities.ComplexTypes;
using VueJS.Entities.Concrete;
using VueJS.Mvc.Areas.Admin.Models;
using VueJS.Shared.Utilities.Results.ComplexTypes;

namespace VueJS.Mvc.Areas.Admin.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TermController : BaseController
    {
        private readonly ITermService _termService;
        private readonly ISeoService _seoService;

        public TermController(ITermService termService, ISeoService seoService, UserManager<User> userManager, IMapper mapper) : base(userManager, mapper)
        {
            _termService = termService;
            _seoService = seoService;
        }

        [HttpGet("/admin/term-allterms")]
        public async Task<JsonResult> AllTerms(SubObjectType termType)
        {
            return Json(await _termService.GetAllAsync(termType));
        }

        [HttpGet("/admin/term-parentlist")]
        public async Task<JsonResult> GetParentList(int? termId)
        {
            return Json(await _termService.GetAllParentAsync(termId));
        }

        [HttpPost("/admin/term-new")]
        public async Task<JsonResult> New(TermViewModel termViewModel)
        {
            if (string.IsNullOrEmpty(termViewModel.SeoObjectSettingAddDto.SeoTitle))
            {
                termViewModel.SeoObjectSettingAddDto.SeoTitle = termViewModel.TermAddDto.Name;
            }
            //await FileHelper.CreateSitemapInRootDirectoryAsync();

            var result = await _termService.AddAsync(termViewModel.TermAddDto);
            return Json(new TermViewModel
            {
                TermDto = await _termService.AddAsync(termViewModel.TermAddDto),
                SeoObjectSettingDto = await _seoService.SeoObjectSettingAddAsync(ObjectType.term, termViewModel.TermAddDto.TermType, result.Data.Term.Id, termViewModel.SeoObjectSettingAddDto, LoggedInUser.Id)
            });
        }

        [HttpPost("/admin/term/newpostterm")]
        public async Task<JsonResult> NewPostTerm(PostTermViewModel postTermViewModel)
        {
            return Json(new PostTermViewModel { PostTermDto = await _termService.PostTermAddAsync(postTermViewModel.PostTermAddDto) });
        }

        [HttpGet("/admin/term-edit")]
        public async Task<JsonResult> Edit(int termId)
        {
            var termResult = await _termService.GetTermUpdateDtoAsync(termId);
            var seoGeneralResult = await _seoService.GetSeoGeneralSettingUpdateDtoAsync();
            return Json(new TermViewModel
            {
                TermUpdateDto = termResult,
                IsActiveCategorySeoSetting = seoGeneralResult.Data.IsActiveCategorySeoSetting,
                IsActiveTagSeoSetting = seoGeneralResult.Data.IsActiveTagSeoSetting,
                SeoObjectSettingUpdateDto = await _seoService.GetSeoObjectSettingUpdateDtoAsync(termId, termResult.Data.TermType)
            });
        }

        [HttpPost("/admin/term-edit")]
        public async Task<JsonResult> Edit(TermViewModel termViewModel)
        {
            await _seoService.SeoObjectSettingUpdateAsync(termViewModel.TermUpdateDto.Data.Id, termViewModel.TermUpdateDto.Data.TermType, termViewModel.SeoObjectSettingUpdateDto.Data, LoggedInUser.Id);
            return Json(new TermViewModel { TermDto = await _termService.UpdateAsync(termViewModel.TermUpdateDto.Data) });
        }

        [HttpPost("/admin/term-delete")]
        public async Task<IActionResult> Delete(int termId)
        {
            return Json(await _termService.DeleteAsync(termId));
            //await FileHelper.CreateSitemapInRootDirectoryAsync();
        }

        [HttpPost("/admin/term-deletepostterm")]
        public async Task<IActionResult> DeletePostTerm(PostTermViewModel postTermViewModel)
        {
            return Json(await _termService.PostTermDeleteAsync(postTermViewModel.PostId, postTermViewModel.TermId));
        }
    }
}