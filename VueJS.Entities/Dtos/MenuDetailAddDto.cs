using VueJS.Entities.Concrete;
using VueJS.Entities.ComplexTypes;

namespace VueJS.Entities.Dtos
{
    public class MenuDetailAddDto
    {
        public int? ParentId { get; set; }
        public int MenuId { get; set; }
        public int? ObjectId { get; set; }
        public SubObjectType? SubObjectType { get; set; }
        public int MenuOrder { get; set; }
        public MenuDetail Parent { get; set; }
    }
}