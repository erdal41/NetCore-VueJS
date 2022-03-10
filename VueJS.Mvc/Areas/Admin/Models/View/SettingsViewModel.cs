using VueJS.Entities.Concrete;
using VueJS.Entities.Dtos;
using VueJS.Shared.Utilities.Results.Abstract;

namespace VueJS.Mvc.Areas.Admin.Models.View
{
    public class SettingsViewModel
    {
        public IDataResult<GeneralSettingDto> GeneralSettingDto { get; set; }
        public IDataResult<GeneralSettingUpdateDto> GeneralSettingUpdateDto { get; set; }
        public IDataResult<SeoGeneralSettingDto> SeoGeneralSettingDto { get; set; }
        public IDataResult<SeoGeneralSettingUpdateDto> SeoGeneralSettingUpdateDto { get; set; }
        public IDataResult<FileDto> FileDto { get; set; }
        public SmtpSettings SmtpSettings { get; set; }
        public ReCaptcha ReCaptcha { get; set; }
        public ArticleRightSideBarWidgetOptions ArticleRightSideBarWidgetOptions { get; set; }
        public ArticleRightSideBarWidgetOptionsDto ArticleRightSideBarWidgetOptionsDto { get; set; }
    }
}