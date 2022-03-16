using VueJS.Entities.Dtos;

namespace VueJS.Mvc.Areas.Admin.Models.Data
{
    public class MenuDataModel
    {
        public MenuAddDto MenuAddDto { get; set; }
        public MenuUpdateDto MenuUpdateDto { get; set; }
        public MenuDetailAddDto MenuDetailAddDto { get; set; }
        public MenuDetailUpdateDto MenuDetailUpdateDto { get; set; }
    }
}