using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using VueJS.Entities.ComplexTypes;
using VueJS.Shared.Entities.Abstract;

namespace VueJS.Entities.Concrete
{
    public class MenuDetail : IEntity
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public int MenuId { get; set; }
        public int? ObjectId { get; set; }
        public SubObjectType? SubObjectType { get; set; }
        public int MenuOrder { get; set; }

        [NotMapped]
        public object Object { get; set; }
        public Menu Menu { get; set; }
        public MenuDetail Parent { get; set; }
        public List<MenuDetail> Parents { get; set; }
        public ICollection<MenuDetail> Children { get; set; }
    }
}