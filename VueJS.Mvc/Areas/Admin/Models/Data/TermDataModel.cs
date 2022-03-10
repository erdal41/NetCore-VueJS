using VueJS.Entities.Dtos;

namespace VueJS.Mvc.Areas.Admin.Models.Data
{
    public class TermDataModel
    {
        public TermAddDto TermAddDto { get; set; }
        public TermUpdateDto TermUpdateDto { get; set; }
        public SeoObjectSettingAddDto SeoObjectSettingAddDto { get; set; }
        public SeoObjectSettingUpdateDto SeoObjectSettingUpdateDto { get; set; }
        public PostTermAddDto PostTermAddDto { get; set; }
    }
}