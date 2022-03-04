using VueJS.Entities.Dtos;
using VueJS.Shared.Utilities.Results.Abstract;

namespace VueJS.Mvc.Areas.Admin.Models
{
    public class TermViewModel
    {
        public IDataResult<TermDto> TermDto { get; set; }
        public IDataResult<TermListDto> TermListDto { get; set; }
        public IDataResult<TermUpdateDto> TermUpdateDto { get; set; }
        public IDataResult<SeoObjectSettingDto> SeoObjectSettingDto { get; set; }
        public IDataResult<SeoObjectSettingUpdateDto> SeoObjectSettingUpdateDto { get; set; }
        public bool IsActiveCategorySeoSetting { get; set; }
        public bool IsActiveTagSeoSetting { get; set; }
    }
}