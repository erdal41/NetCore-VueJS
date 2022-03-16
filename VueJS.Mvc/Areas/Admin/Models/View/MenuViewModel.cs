using VueJS.Entities.Dtos;
using VueJS.Shared.Utilities.Results.Abstract;

namespace VueJS.Mvc.Areas.Admin.Models.View
{
    public class MenuViewModel
    {
        public IDataResult<MenuDto> MenuDto { get; set; }
        public IDataResult<MenuListDto> MenuListDto { get; set; }
        public IDataResult<MenuUpdateDto> MenuUpdateDto { get; set; }
        public IDataResult<MenuDetailDto> MenuDetailDto { get; set; }
        public IDataResult<MenuDetailListDto> MenuDetailListDto { get; set; }
        public IDataResult<MenuDetailUpdateDto> MenuDetailUpdateDto { get; set; }
    }
}