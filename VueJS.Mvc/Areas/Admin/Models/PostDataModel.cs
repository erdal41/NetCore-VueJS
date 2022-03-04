using VueJS.Entities.Dtos;

namespace VueJS.Mvc.Areas.Admin.Models
{
    public class PostDataModel
    {
        public PostAddDto PostAddDto { get; set; }
        public PostUpdateDto PostUpdateDto { get; set; }
        public SeoObjectSettingAddDto SeoObjectSettingAddDto { get; set; }
        public SeoObjectSettingUpdateDto SeoObjectSettingUpdateDto { get; set; }      
    }
}