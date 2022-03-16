using VueJS.Entities.Concrete;
using VueJS.Shared.Entities.Abstract;
using System.Collections.Generic;

namespace VueJS.Entities.Dtos
{
    public class MenuDetailListDto : DtoBase
    {
        public IList<MenuDetail> MenuDetails { get; set; }
    }
}