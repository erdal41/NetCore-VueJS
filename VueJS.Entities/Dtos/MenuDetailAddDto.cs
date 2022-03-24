using VueJS.Entities.Concrete;
using VueJS.Entities.ComplexTypes;

namespace VueJS.Entities.Dtos
{
    public class MenuDetailAddDto
    {
        public string CustomName { get; set; }
        public string CustomURL { get; set; }
        public int? ParentId { get; set; }
        public int MenuId { get; set; }
        public int? ObjectId { get; set; }
        public ObjectType? ObjectType { get; set; }
        public int MenuOrder { get; set; }
        public MenuDetail Parent { get; set; }
    }
}