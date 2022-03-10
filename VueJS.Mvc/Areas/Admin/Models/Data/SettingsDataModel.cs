using VueJS.Entities.Dtos;

namespace VueJS.Mvc.Areas.Admin.Models.Data
{
    public class SettingsDataModel
    {
        public GeneralSettingUpdateDto GeneralSettingUpdateDto { get; set; }
        public SeoGeneralSettingUpdateDto SeoGeneralSettingUpdateDto { get; set; }
        public FileDto FileDto { get; set; }
    }
}