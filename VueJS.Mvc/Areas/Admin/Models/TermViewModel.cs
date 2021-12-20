using VueJS.Entities.Dtos;

namespace VueJS.Mvc.Areas.Admin.Models
{
    public class TermViewModel
    {
        public TermAddDto TermAddDto { get; set; }
        public TermDto TermDto { get; set; }
        public SeoObjectSettingDto SeoObjectSettingDto { get; set; }
        public SeoObjectSettingAddDto SeoObjectSettingAddDto { get; set; }

        public TermUpdateDto TermUpdateDto { get; set; }
        public bool IsActiveCategorySeoSetting { get; set; }
        public bool IsActiveTagSeoSetting { get; set; }
        public SeoObjectSettingUpdateDto SeoObjectSettingUpdateDto { get; set; }
    }
}