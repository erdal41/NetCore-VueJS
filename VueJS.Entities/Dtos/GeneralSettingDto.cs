using VueJS.Entities.Concrete;
using VueJS.Shared.Entities.Abstract;

namespace VueJS.Entities.Dtos
{
    public class GeneralSettingDto : DtoResultBase
    {
        public GeneralSetting GeneralSetting { get; set; }
    }
}