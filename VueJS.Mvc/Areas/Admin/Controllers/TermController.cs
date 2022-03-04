using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AutoMapper;
using VueJS.Services.Abstract;
using VueJS.Entities.ComplexTypes;
using VueJS.Entities.Concrete;
using VueJS.Mvc.Areas.Admin.Models;
using VueJS.Shared.Utilities.Results.ComplexTypes;
using VueJS.Entities.Dtos;

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
            return Json(new TermViewModel { TermListDto = await _termService.GetAllAsync(termType) });
        }

        [HttpGet("/admin/term-parentlist")]
        public async Task<JsonResult> GetParentList(int? termId)
        {
            return Json(new TermViewModel { TermListDto = await _termService.GetAllParentAsync(termId) });
        }

        [HttpPost("/admin/term-new")]
        public async Task<JsonResult> New(TermDataModel termDataModel)
        {
            if (string.IsNullOrEmpty(termDataModel.SeoObjectSettingAddDto.SeoTitle))
            {
                termDataModel.SeoObjectSettingAddDto.SeoTitle = termDataModel.TermAddDto.Name;
            }
            //await FileHelper.CreateSitemapInRootDirectoryAsync();

            var result = await _termService.AddAsync(termDataModel.TermAddDto);
            return Json(new TermViewModel
            {
                TermDto = await _termService.AddAsync(termDataModel.TermAddDto),
                SeoObjectSettingDto = await _seoService.SeoObjectSettingAddAsync(ObjectType.term, termDataModel.TermAddDto.TermType, result.Data.Term.Id, termDataModel.SeoObjectSettingAddDto, LoggedInUser.Id)
            });
        }

        [HttpPost("/admin/term/newpostterm")]
        public async Task<JsonResult> NewPostTerm(PostTermAddDto postTermAddDto)
        {
            return Json(new PostTermViewModel { PostTermDto = await _termService.PostTermAddAsync(postTermAddDto) });
        }

        [HttpGet("/admin/term-edit")]
        public async Task<JsonResult> Edit(int termId)
        {
            var termResult = await _termService.GetTermUpdateDtoAsync(termId);
            var seoGeneralResult = await _seoService.GetSeoGeneralSettingUpdateDtoAsync();
            if (termResult.ResultStatus != ResultStatus.Success && seoGeneralResult.ResultStatus != ResultStatus.Success) return Json(new TermViewModel { TermUpdateDto = termResult });

            return Json(new TermViewModel
            {
                TermUpdateDto = termResult,
                IsActiveCategorySeoSetting = seoGeneralResult.Data.IsActiveCategorySeoSetting,
                IsActiveTagSeoSetting = seoGeneralResult.Data.IsActiveTagSeoSetting,
                SeoObjectSettingUpdateDto = await _seoService.GetSeoObjectSettingUpdateDtoAsync(termId, termResult.Data.TermType)
            });
        }

        [HttpPost("/admin/term-edit")]
        public async Task<JsonResult> Edit(TermDataModel termDataModel)
        {
            var termResult = await _termService.UpdateAsync(termDataModel.TermUpdateDto);
            var seoResult = termResult.ResultStatus == ResultStatus.Success ? await _seoService.SeoObjectSettingUpdateAsync(termDataModel.TermUpdateDto.Id, termDataModel.TermUpdateDto.TermType, termDataModel.SeoObjectSettingUpdateDto, LoggedInUser.Id) : null;
            return Json(new TermViewModel
            {
                TermDto = termResult,
                SeoObjectSettingDto = seoResult
            });
        }

        [HttpPost("/admin/term-delete")]
        public async Task<IActionResult> Delete(int termId)
        {
            return Json(await _termService.DeleteAsync(termId));
            //await FileHelper.CreateSitemapInRootDirectoryAsync();
        }

        [HttpPost("/admin/term-deletepostterm")]
        public async Task<IActionResult> DeletePostTerm(int postId, int termId)
        {
            return Json(await _termService.PostTermDeleteAsync(postId, termId));
        }
    }
}