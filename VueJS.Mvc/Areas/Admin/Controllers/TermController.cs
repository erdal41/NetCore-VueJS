using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using System;
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

        [HttpGet("/admin/term/allterms")]
        [Route("/admin/term/allterms")]
        public async Task<JsonResult> AllTerms(SubObjectType term_type)
        {
            var result = await _termService.GetAllAsync(term_type, null);
            return new JsonResult(result.Data);
        }

        [HttpGet]
        [Route("/admin/term/getparentlist")]
        public async Task<JsonResult> GetParentList(int? termId, string search)
        {
            var result = await _termService.GetAllParentAsync(termId, search);
            return new JsonResult(result.Data);
        }

        [HttpPost]
        [Route("/admin/term/new")]
        public async Task<JsonResult> New(TermViewModel termViewModel)
        {
            var termResult = await _termService.AddAsync(termViewModel.TermAddDto);
            termViewModel.SeoObjectSettingAddDto.SeoTitle = termViewModel.TermAddDto.Name;
            if (termResult.Data.ResultStatus == ResultStatus.Success)
            {
                var seoResult = await _seoService.SeoObjectSettingAddAsync(ObjectType.term, termViewModel.TermAddDto.TermType, termResult.Data.Term.Id, termViewModel.SeoObjectSettingAddDto, 1);
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

        [HttpGet]
        [Route("/admin/term/edit")]
        public async Task<JsonResult> Edit([FromQuery] int term)
        {
            var termResult = await _termService.GetTermUpdateDtoAsync(term);
            var seoGeneralResult = await _seoService.GetSeoGeneralSettingUpdateDtoAsync();
            if (termResult.ResultStatus == ResultStatus.Success && seoGeneralResult.ResultStatus == ResultStatus.Success)
            {
                var seoResult = await _seoService.GetSeoObjectSettingUpdateDtoAsync(term, termResult.Data.TermType);
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

        [HttpPost]
        [Route("/admin/term/edit")]
        public async Task<JsonResult> Edit(TermViewModel termViewModel)
        {
            var termResult = await _termService.UpdateAsync(termViewModel.TermUpdateDto);

            if (termResult.ResultStatus == ResultStatus.Success)
            {
                await _seoService.SeoObjectSettingUpdateAsync(termViewModel.TermUpdateDto.Id, termViewModel.TermUpdateDto.TermType, termViewModel.SeoObjectSettingUpdateDto, 1);

                var termViewModelJson = new TermViewModel
                {
                    TermDto = termResult.Data
                };
                return new JsonResult(termViewModelJson);
            }
            var termViewModelJsonError = new TermViewModel
            {
                TermDto = termResult.Data
            };
            return new JsonResult(termViewModelJsonError);
        }

        [HttpPost]
        [Route("/admin/term/delete")]
        public async Task<IActionResult> Delete([FromQuery] int term)
        {
            var result = await _termService.DeleteAsync(term);
            //await FileHelper.CreateSitemapInRootDirectoryAsync();
            return new JsonResult(result);
        }

        [HttpPost]
        [Route("/admin/term/multidelete")]
        public async Task<IActionResult> MultiDelete(List<int> terms)
        {
            List<int> s = null;
            foreach (var item in terms)
            {
                s.Add(Convert.ToInt32(item));
            }
            var result = await _termService.MultiDeleteAsync(terms);
            //if (result.ResultStatus == ResultStatus.Success)
            //{
            //    await FileHelper.CreateSitemapInRootDirectoryAsync();
            //}
            return new JsonResult(result);
        }

        //[Authorize(Roles = "SuperAdmin,Category.Create")]
        //[HttpPost]
        //[Route("admin/term/newpostterm")]
        //public async Task<JsonResult> NewPostTerm(PostTermViewModel postTermViewModel)
        //{
        //    var result = await _termService.PostTermAddAsync(postTermViewModel.PostTermAddDto);
        //    if (result.ResultStatus == ResultStatus.Success)
        //    {
        //        var postTermAjaxViewModel = JsonConvert.SerializeObject(new PostTermViewModel
        //        {
        //            PostTermDto = result.Data,
        //        });
        //        return Json(postTermAjaxViewModel);
        //    }
        //    else
        //    {
        //        var postTermAjaxViewModel = JsonConvert.SerializeObject(new PostTermViewModel
        //        {
        //            PostTermDto = result.Data,
        //        });
        //        return Json(postTermAjaxViewModel);
        //    }
        //}
    }
}