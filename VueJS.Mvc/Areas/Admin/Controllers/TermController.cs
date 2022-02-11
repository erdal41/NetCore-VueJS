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
        public async Task<JsonResult> AllTerms(SubObjectType term_type)
        {
            var result = await _termService.GetAllAsync(term_type);
            return new JsonResult(result.Data);
        }

        [HttpGet("/admin/term-parentlist")]
        public async Task<JsonResult> GetParentList(int? termId)
        {
            var result = await _termService.GetAllParentAsync(termId);
            return new JsonResult(result.Data);
        }

        [HttpPost("/admin/term-new")]
        public async Task<JsonResult> New(TermViewModel termViewModel)
        {
            var termResult = await _termService.AddAsync(termViewModel.TermAddDto);
            if (string.IsNullOrEmpty(termViewModel.SeoObjectSettingAddDto.SeoTitle))
            {
                termViewModel.SeoObjectSettingAddDto.SeoTitle = termViewModel.TermAddDto.Name;
            }
            if (termResult.Data.ResultStatus == ResultStatus.Success)
            {
                var seoResult = await _seoService.SeoObjectSettingAddAsync(ObjectType.term, termViewModel.TermAddDto.TermType, termResult.Data.Term.Id, termViewModel.SeoObjectSettingAddDto, LoggedInUser.Id);
                //await FileHelper.CreateSitemapInRootDirectoryAsync();
                var termViewModelJson = new TermViewModel
                {
                    TermDto = termResult.Data,
                    SeoObjectSettingDto = seoResult.Data
                };
                return new JsonResult(termViewModelJson);
            }
            else
            {
                var termViewModelJsonError = new TermViewModel
                {
                    TermDto = termResult.Data
                };
                return new JsonResult(termViewModelJsonError);
            }
        }

        [HttpPost("/admin/term/newpostterm")]
        public async Task<JsonResult> NewPostTerm(PostTermViewModel postTermViewModel)
        {
            var result = await _termService.PostTermAddAsync(postTermViewModel.PostTermAddDto);
            var postTermViewModelJson = new PostTermViewModel
            {
                PostTermDto = result.Data,
            };
            return new JsonResult(postTermViewModelJson);
        }

        [HttpGet("/admin/term-edit")]
        public async Task<JsonResult> Edit( int termId)
        {
            var termResult = await _termService.GetTermUpdateDtoAsync(termId);
            var seoGeneralResult = await _seoService.GetSeoGeneralSettingUpdateDtoAsync();
            if (termResult.ResultStatus == ResultStatus.Success && seoGeneralResult.ResultStatus == ResultStatus.Success)
            {
                var seoResult = await _seoService.GetSeoObjectSettingUpdateDtoAsync(termId, termResult.Data.TermType);
                var termViewModelJson = new TermViewModel
                {
                    TermUpdateDto = termResult.Data,
                    IsActiveCategorySeoSetting = seoGeneralResult.Data.IsActiveCategorySeoSetting,
                    IsActiveTagSeoSetting = seoGeneralResult.Data.IsActiveTagSeoSetting,
                    SeoObjectSettingUpdateDto = seoResult.Data
                };
                return new JsonResult(termViewModelJson);
            }
            var termViewModelJsonError = new TermViewModel
            {
                TermUpdateDto = termResult.Data
            };
            return new JsonResult(termViewModelJsonError);
        }

        [HttpPost("/admin/term-edit")]
        public async Task<JsonResult> Edit(TermViewModel termViewModel)
        {
            var termResult = await _termService.UpdateAsync(termViewModel.TermUpdateDto);

            if (termResult.ResultStatus == ResultStatus.Success)
            {
                await _seoService.SeoObjectSettingUpdateAsync(termViewModel.TermUpdateDto.Id, termViewModel.TermUpdateDto.TermType, termViewModel.SeoObjectSettingUpdateDto, LoggedInUser.Id);

                var termViewModelJson = new TermViewModel
                {
                    TermDto = termResult.Data,
                };
                return new JsonResult(termViewModelJson);
            }
            var termViewModelJsonError = new TermViewModel
            {
                TermDto = termResult.Data
            };
            return new JsonResult(termViewModelJsonError);
        }

        [HttpPost("/admin/term-delete")]
        public async Task<IActionResult> Delete( int termId)
        {
            var result = await _termService.DeleteAsync(termId);
            //await FileHelper.CreateSitemapInRootDirectoryAsync();
            return new JsonResult(result);
        }

        [HttpPost("/admin/term-deletepostterm")]
        public async Task<IActionResult> DeletePostTerm(PostTermViewModel postTermViewModel )
        {
            var result = await _termService.PostTermDeleteAsync(postTermViewModel.PostId, postTermViewModel.TermId);
            return new JsonResult(result);
        }
    }
}