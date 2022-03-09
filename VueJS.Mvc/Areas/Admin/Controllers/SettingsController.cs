using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using VueJS.Entities.ComplexTypes;
using VueJS.Entities.Concrete;
using VueJS.Entities.Dtos;
using VueJS.Mvc.Areas.Admin.Models;
using VueJS.Mvc.Helpers.Abstract;
using VueJS.Services.Abstract;
using VueJS.Shared.Utilities.Helpers.Abstract;
using System.Threading.Tasks;
using VueJS.Shared.Utilities.Results.ComplexTypes;

namespace VueJS.Mvc.Areas.Admin.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SettingsController : BaseController
    {
        private readonly SmtpSettings _smtpSettings;
        private readonly IWritableOptions<SmtpSettings> _smtpSettingsWriter;
        private readonly ReCaptcha _reCaptcha;
        private readonly IWritableOptions<ReCaptcha> _reCaptchaWriter;
        private readonly ArticleRightSideBarWidgetOptions _articleRightSideBarWidgetOptions;
        private readonly IWritableOptions<ArticleRightSideBarWidgetOptions> _articleRightSideBarWidgetOptionsWriter;
        private readonly ISettingService _settingService;
        private readonly ITermService _termService;

        public SettingsController(IOptionsSnapshot<SmtpSettings> smtpSettings, IWritableOptions<SmtpSettings> smtpSettingsWriter, IOptionsSnapshot<ReCaptcha> reCaptcha, IWritableOptions<ReCaptcha> reCaptchaWriter, IOptionsSnapshot<ArticleRightSideBarWidgetOptions> articleRightSideBarWidgetOptions, IWritableOptions<ArticleRightSideBarWidgetOptions> articleRightSideBarWidgetOptionsWriter, ISettingService settingService, ITermService termService, UserManager<User> userManager, IMapper mapper, IFileHelper fileHelper) : base(userManager, mapper, fileHelper)
        {
            _smtpSettings = smtpSettings.Value;
            _smtpSettingsWriter = smtpSettingsWriter;
            _reCaptcha = reCaptcha.Value;
            _reCaptchaWriter = reCaptchaWriter;
            _articleRightSideBarWidgetOptions = articleRightSideBarWidgetOptions.Value;
            _articleRightSideBarWidgetOptionsWriter = articleRightSideBarWidgetOptionsWriter;
            _settingService = settingService;
            _termService = termService;
        }

        [HttpGet("admin/settings-general")]
        public async Task<JsonResult> General()
        {
            return Json(new GeneralSettingViewModel { GeneralSettingUpdateDto = await _settingService.GetGeneralSettingUpdateDtoAsync() });
        }

        [HttpPost("admin/settings-general")]
        public async Task<JsonResult> General(GeneralSettingUpdateDto generalSettingUpdateDto)
        {
            return Json(new GeneralSettingViewModel { GeneralSettingDto = await _settingService.GeneralSettingUpdateAsync(generalSettingUpdateDto, LoggedInUser.Id) });
        }

        [HttpGet("admin/settings-getjsfileread")]
        public JsonResult GetJsFileRead()
        {
            return Json(new FileViewModel { FileDto = FileHelper.GetJsFileRead("custom.js") });
        }

        [HttpPost("admin/settings-jsfileupdate")]
        public JsonResult JsFileUpdate(string text)
        {
            return Json(new FileViewModel { FileDto = FileHelper.JsFileUpdate("custom.js", text) });
        }

        [HttpGet("admin/settings-getcssfileread")]
        public JsonResult GetCssFileRead()
        {
            return Json(new FileViewModel { FileDto = FileHelper.GetCssFileRead("custom.css") });

        }

        [HttpPost("admin/settings-cssfileupdate")]
        public JsonResult CssFileUpdate(string text)
        {
            return Json(new FileViewModel { FileDto = FileHelper.CssFileUpdate("custom.css", text) });
        }

        [HttpGet("admin/settings-email")]
        public JsonResult Email()
        {
            return Json(_smtpSettings);
        }

        [HttpPost("admin/settings-email")]
        public JsonResult Email(SmtpSettings smtpSettings)
        {
            _smtpSettingsWriter.Update(x =>
            {
                x.Server = smtpSettings.Server;
                x.Port = smtpSettings.Port;
                x.SenderEmail = smtpSettings.SenderEmail;
                x.Username = smtpSettings.Username;
                x.Password = smtpSettings.Password;
                x.IsEnableSsl = smtpSettings.IsEnableSsl;
            });
            return Json(smtpSettings);
        }

        [HttpGet("admin/settings-recaptcha")]
        public JsonResult ReCaptcha()
        {
            return Json(_reCaptcha);
        }

        [HttpPost("admin/settings-recaptcha")]
        public JsonResult ReCaptcha(ReCaptcha reCaptcha)
        {
            _reCaptchaWriter.Update(x =>
            {
                x.SiteKey = reCaptcha.SiteKey;
                x.SecretKey = reCaptcha.SecretKey;
            });
            return Json(reCaptcha);
        }

        [HttpGet("admin/settings-blog")]
        public JsonResult Blog()
        {
            return Json(_smtpSettings);
        }

        [HttpGet("admin/settings-articlerightsidebar")]
        public async Task<JsonResult> ArticleRightSideBarWidgetSettings()
        {
            var categoriesResult = await _termService.GetAllAsync(SubObjectType.category);
            var articleRightSideBarWidgetOptionsViewModel = Mapper.Map<ArticleRightSideBarWidgetOptionsViewModel>(_articleRightSideBarWidgetOptions);
            articleRightSideBarWidgetOptionsViewModel.Terms = categoriesResult.Data.Terms;
            return Json(articleRightSideBarWidgetOptionsViewModel);
        }

        [HttpPost("admin/settings-articlerightsidebar")]
        public async Task<JsonResult> ArticleRightSideBarWidgetSettings(ArticleRightSideBarWidgetOptionsViewModel articleRightSideBarWidgetOptionsViewModel)
        {
            var categoriesResult = await _termService.GetAllAsync(SubObjectType.category);
            if(categoriesResult.ResultStatus != ResultStatus.Success) return Json(categoriesResult);
            articleRightSideBarWidgetOptionsViewModel.Terms = categoriesResult.Data.Terms;

            _articleRightSideBarWidgetOptionsWriter.Update(x =>
            {
                x.Header = articleRightSideBarWidgetOptionsViewModel.Header;
                x.TakeSize = articleRightSideBarWidgetOptionsViewModel.TakeSize;
                x.CategoryId = articleRightSideBarWidgetOptionsViewModel.CategoryId;
                x.FilterBy = articleRightSideBarWidgetOptionsViewModel.FilterBy;
                x.OrderBy = articleRightSideBarWidgetOptionsViewModel.OrderBy;
                x.IsAscending = articleRightSideBarWidgetOptionsViewModel.IsAscending;
                x.StartAt = articleRightSideBarWidgetOptionsViewModel.StartAt;
                x.EndAt = articleRightSideBarWidgetOptionsViewModel.EndAt;
                x.MaxViewCount = articleRightSideBarWidgetOptionsViewModel.MaxViewCount;
                x.MinViewCount = articleRightSideBarWidgetOptionsViewModel.MinViewCount;
                x.MaxCommentCount = articleRightSideBarWidgetOptionsViewModel.MaxCommentCount;
                x.MinCommentCount = articleRightSideBarWidgetOptionsViewModel.MinCommentCount;
            });
            return Json(articleRightSideBarWidgetOptionsViewModel);
        }
    }
}
