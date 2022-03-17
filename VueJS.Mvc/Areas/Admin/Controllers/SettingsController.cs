using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using VueJS.Entities.ComplexTypes;
using VueJS.Entities.Concrete;
using VueJS.Mvc.Helpers.Abstract;
using VueJS.Services.Abstract;
using VueJS.Shared.Utilities.Helpers.Abstract;
using System.Threading.Tasks;
using VueJS.Shared.Utilities.Results.ComplexTypes;
using VueJS.Mvc.Areas.Admin.Models.View;
using VueJS.Mvc.Areas.Admin.Models.Data;
using System.Linq;
using System;
using VueJS.Shared.Utilities.Extensions;
using VueJS.Entities.Dtos;

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
        private readonly ISeoService _seoService;
        private readonly ITermService _termService;

        public SettingsController(
            IOptionsSnapshot<SmtpSettings> smtpSettings,
            IWritableOptions<SmtpSettings> smtpSettingsWriter,
            IOptionsSnapshot<ReCaptcha> reCaptcha,
            IWritableOptions<ReCaptcha> reCaptchaWriter,
            IOptionsSnapshot<ArticleRightSideBarWidgetOptions> articleRightSideBarWidgetOptions,
            IWritableOptions<ArticleRightSideBarWidgetOptions> articleRightSideBarWidgetOptionsWriter,
            ISettingService settingService,
            ISeoService seoService,
            ITermService termService,
            UserManager<User> userManager,
            IMapper mapper,
            IFileHelper fileHelper) : base(userManager, mapper, fileHelper)
        {
            _smtpSettings = smtpSettings.Value;
            _smtpSettingsWriter = smtpSettingsWriter;
            _reCaptcha = reCaptcha.Value;
            _reCaptchaWriter = reCaptchaWriter;
            _articleRightSideBarWidgetOptions = articleRightSideBarWidgetOptions.Value;
            _articleRightSideBarWidgetOptionsWriter = articleRightSideBarWidgetOptionsWriter;
            _settingService = settingService;
            _seoService = seoService;
            _termService = termService;
        }

        #region GENERAL

        [HttpGet("/admin/settings-general")]
        public async Task<JsonResult> General()
        {
            return Json(new SettingsViewModel { GeneralSettingUpdateDto = await _settingService.GetGeneralSettingUpdateDtoAsync() });
        }

        [HttpPost("/admin/settings-general")]
        public async Task<JsonResult> General(SettingsDataModel settingsDataModel)
        {
            return Json(new SettingsViewModel { GeneralSettingDto = await _settingService.GeneralSettingUpdateAsync(settingsDataModel.GeneralSettingUpdateDto, LoggedInUser.Id) });
        }

        [HttpGet("/admin/settings-getjsfileread")]
        public JsonResult GetJsFileRead()
        {
            return Json(new SettingsViewModel { FileDto = FileHelper.GetJsFileRead("custom.js") });
        }

        [HttpPost("/admin/settings-jsfileupdate")]
        public JsonResult JsFileUpdate(string text)
        {
            return Json(new SettingsViewModel { FileDto = FileHelper.JsFileUpdate("custom.js", text) });
        }

        [HttpGet("/admin/settings-getcssfileread")]
        public JsonResult GetCssFileRead()
        {
            return Json(new SettingsViewModel { FileDto = FileHelper.GetCssFileRead("custom.css") });

        }

        [HttpPost("/admin/settings-cssfileupdate")]
        public JsonResult CssFileUpdate(string text)
        {
            return Json(new SettingsViewModel { FileDto = FileHelper.CssFileUpdate("custom.css", text) });
        }

        #endregion

        #region SEO

        [HttpGet("/admin/settings-seogeneral")]
        public async Task<JsonResult> SeoGeneralSettings()
        {
            return Json(new SettingsViewModel { SeoGeneralSettingUpdateDto = await _seoService.GetSeoGeneralSettingUpdateDtoAsync() });
        }

        [HttpPost("/admin/settings-seogeneral")]
        public async Task<JsonResult> SeoGeneralSettings(SettingsDataModel settingsDataModel)
        {
            var result = await _seoService.SeoGeneralSettingUpdateAsync(settingsDataModel.SeoGeneralSettingUpdateDto, LoggedInUser.Id);
            if (result.ResultStatus == ResultStatus.Success)
            {
                if (result.Data.SeoGeneralSetting.IsActiveRobotsTxt == false)
                {
                    FileHelper.Delete("robots.txt");
                }

                if (result.Data.SeoGeneralSetting.IsActiveSitemap == false)
                {
                    FileHelper.Delete("sitemap.xml");
                }
                else
                {
                    await FileHelper.CreateSitemapInRootDirectoryAsync();
                }
            }
            return Json(new SettingsViewModel { SeoGeneralSettingDto = result });
        }

        [HttpGet("/admin/settings-getseotype")]
        public JsonResult GetSeoType()
        {
            return Json(Enum.GetValues(typeof(SeoType))
                .Cast<SeoType>()
                .Select(spt => new EnumViewModel
                {
                    Id = (int)spt,
                    Name = spt.GetDisplayName()
                }));
        }

        [HttpGet("/admin/settings-getschemapagetype")]
        public JsonResult GetSchemaPageType()
        {
            return Json(Enum.GetValues(typeof(SchemaPageType))
                .Cast<SchemaPageType>()
                .Select(spt => new EnumViewModel
                {
                    Id = (int)spt,
                    Name = spt.GetDisplayName()
                }));
        }

        [HttpGet("/admin/settings-getschemaarticletype")]
        public JsonResult GetSchemaArticleType()
        {
            return Json(Enum.GetValues(typeof(SchemaArticleType))
                .Cast<SchemaArticleType>()
                .Select(spt => new EnumViewModel
                {
                    Id = (int)spt,
                    Name = spt.GetDisplayName()
                }));
        }

        [HttpGet("/admin/settings-gettwittercardtype")]
        public JsonResult GetTwitterCardType()
        {
            return Json(Enum.GetValues(typeof(TwitterCardType))
                .Cast<TwitterCardType>()
                .Select(spt => new EnumViewModel
                {
                    Id = (int)spt,
                    Name = spt.GetDisplayName()
                }));
        }

        [HttpGet("/admin/settings-seorobotstxt")]
        public JsonResult RobotsTxt()
        {
            var result = FileHelper.GetFileRead("robots.txt");
            if (result.Data.Text == null)
            {
                result.Data.Text = FileHelper.DefaultRobotsTxt();
            }
            return Json(new SettingsViewModel { FileDto = result });
        }

        [HttpPost("/admin/settings-seorobotstxt")]
        public JsonResult RobotsTxt(SettingsDataModel settingsDataModel)
        {
            return Json(new SettingsViewModel { FileDto = FileHelper.FileAddOrUpdate("robots.txt", settingsDataModel.FileDto.Text) });
        }

        [Route("/admin/settings-seorobotstxtdelete")]
        public JsonResult RobotsTxtDelete()
        {
            return Json(new SettingsViewModel { FileDto = FileHelper.Delete("robots.txt") });
        }

        #endregion

        #region FORM

        [HttpGet("/admin/settings-form")]
        public JsonResult Form()
        {
            return Json(new SettingsViewModel
            {
                SmtpSettings = _smtpSettings,
                ReCaptcha = _reCaptcha
            });
        }

        [HttpPost("/admin/settings-form")]
        public JsonResult Form(SettingsViewModel settingsViewModel)
        {
            if (settingsViewModel.SmtpSettings != null)
            {
                _smtpSettingsWriter.Update(x =>
                {
                    x.Server = settingsViewModel.SmtpSettings.Server;
                    x.Port = settingsViewModel.SmtpSettings.Port;
                    x.SenderEmail = settingsViewModel.SmtpSettings.SenderEmail;
                    x.Username = settingsViewModel.SmtpSettings.Username;
                    x.Password = settingsViewModel.SmtpSettings.Password;
                    x.IsEnableSsl = settingsViewModel.SmtpSettings.IsEnableSsl;
                });
            }

            if (settingsViewModel.ReCaptcha != null)
            {
                _reCaptchaWriter.Update(x =>
                {
                    x.SiteKey = settingsViewModel.ReCaptcha.SiteKey;
                    x.SecretKey = settingsViewModel.ReCaptcha.SecretKey;
                });
            }

            return Json(new SettingsViewModel
            {
                SmtpSettings = settingsViewModel.SmtpSettings,
                ReCaptcha = settingsViewModel.ReCaptcha
            });
        }

        #endregion

        #region WIDGET

        [HttpGet("/admin/settings-getfilter")]
        public JsonResult GetFilter()
        {
            return Json(Enum.GetValues(typeof(FilterBy))
                .Cast<FilterBy>()
                .Select(spt => new EnumViewModel
                {
                    Id = (int)spt,
                    Name = spt.GetDisplayName()
                }));
        }

        [HttpGet("/admin/settings-getorder")]
        public JsonResult GetOrder()
        {
            return Json(Enum.GetValues(typeof(OrderBy))
                .Cast<OrderBy>()
                .Select(spt => new EnumViewModel
                {
                    Id = (int)spt,
                    Name = spt.GetDisplayName()
                }));
        }

        [HttpGet("/admin/settings-widget")]
        public async Task<JsonResult> ArticleRightSideBarWidgetSettings()
        {
            var categories = await _termService.GetAllAsync(ObjectType.category);
            var tags = await _termService.GetAllAsync(ObjectType.tag);
            if (categories.ResultStatus != ResultStatus.Success && tags.ResultStatus != ResultStatus.Success) return Json(new SettingsViewModel { ArticleRightSideBarWidgetOptionsDto = null });

            var articleRightSideBarWidgetOptionsDto = Mapper.Map<ArticleRightSideBarWidgetOptionsDto>(_articleRightSideBarWidgetOptions);
            articleRightSideBarWidgetOptionsDto.Categories = categories.Data.Terms;
            articleRightSideBarWidgetOptionsDto.Tags = tags.Data.Terms;
            return Json(new SettingsViewModel { ArticleRightSideBarWidgetOptionsDto = articleRightSideBarWidgetOptionsDto });
        }

        [HttpPost("/admin/settings-widget")]
        public async Task<JsonResult> ArticleRightSideBarWidgetSettings(SettingsViewModel settingsViewModel)
        {
            var categories = await _termService.GetAllAsync(ObjectType.category);
            var tags = await _termService.GetAllAsync(ObjectType.tag);
            if (categories.ResultStatus != ResultStatus.Success && tags.ResultStatus != ResultStatus.Success && settingsViewModel.ArticleRightSideBarWidgetOptions == null) return Json(settingsViewModel);
            settingsViewModel.ArticleRightSideBarWidgetOptionsDto.Categories = categories.Data.Terms;
            settingsViewModel.ArticleRightSideBarWidgetOptionsDto.Tags = tags.Data.Terms;

            _articleRightSideBarWidgetOptionsWriter.Update(x =>
            {
                x.Header = settingsViewModel.ArticleRightSideBarWidgetOptionsDto.Header;
                x.TakeSize = settingsViewModel.ArticleRightSideBarWidgetOptionsDto.TakeSize;
                x.CategoryId = settingsViewModel.ArticleRightSideBarWidgetOptionsDto.CategoryId;
                x.TagId = settingsViewModel.ArticleRightSideBarWidgetOptionsDto.TagId;
                x.FilterBy = settingsViewModel.ArticleRightSideBarWidgetOptionsDto.FilterBy;
                x.OrderBy = settingsViewModel.ArticleRightSideBarWidgetOptionsDto.OrderBy;
                x.IsAscending = settingsViewModel.ArticleRightSideBarWidgetOptionsDto.IsAscending;
                x.StartAt = settingsViewModel.ArticleRightSideBarWidgetOptionsDto.StartAt;
                x.EndAt = settingsViewModel.ArticleRightSideBarWidgetOptionsDto.EndAt;
                x.MaxViewCount = settingsViewModel.ArticleRightSideBarWidgetOptionsDto.MaxViewCount;
                x.MinViewCount = settingsViewModel.ArticleRightSideBarWidgetOptionsDto.MinViewCount;
                x.MaxCommentCount = settingsViewModel.ArticleRightSideBarWidgetOptionsDto.MaxCommentCount;
                x.MinCommentCount = settingsViewModel.ArticleRightSideBarWidgetOptionsDto.MinCommentCount;
            });
            return Json(new SettingsViewModel { ArticleRightSideBarWidgetOptions = _articleRightSideBarWidgetOptions });
        }

        #endregion

        [HttpGet("/admin/settings-blog")]
        public JsonResult Blog()
        {
            return Json(_smtpSettings);
        }
    }
}
