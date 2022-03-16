using VueJS.Entities.Concrete;
using VueJS.Shared.Entities.Abstract;
using System.Collections.Generic;

namespace VueJS.Entities.Dtos
{
    public class MenuListDto : DtoBase
    {
        public IList<Menu> Menus { get; set; }
    }
}