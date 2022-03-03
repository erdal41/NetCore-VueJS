using VueJS.Entities.Dtos;
using VueJS.Shared.Utilities.Results.Abstract;

namespace VueJS.Mvc.Areas.Admin.Models
{
    public class TermViewModel
    {
        public TermAddDto TermAddDto { get; set; }
        public IDataResult<TermDto> TermDto { get; set; }
        public IDataResult<SeoObjectSettingDto> SeoObjectSettingDto { get; set; }
        public SeoObjectSettingAddDto SeoObjectSettingAddDto { get; set; }

        public IDataResult<TermUpdateDto> TermUpdateDto { get; set; }
        public bool IsActiveCategorySeoSetting { get; set; }
        public bool IsActiveTagSeoSetting { get; set; }
        public IDataResult<SeoObjectSettingUpdateDto> SeoObjectSettingUpdateDto { get; set; }
    }
}