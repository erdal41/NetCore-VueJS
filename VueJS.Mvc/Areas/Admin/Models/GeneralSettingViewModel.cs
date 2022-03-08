using VueJS.Entities.Dtos;
using VueJS.Shared.Utilities.Results.Abstract;

namespace VueJS.Mvc.Areas.Admin.Models
{
    public class GeneralSettingViewModel
    {
        public IDataResult<GeneralSettingDto> GeneralSettingDto { get; set; }
        public IDataResult<GeneralSettingUpdateDto> GeneralSettingUpdateDto { get; set; }
    }
}