using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VueJS.Shared.Entities.Abstract;

namespace VueJS.Entities.Concrete
{
    public class Menu : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<MenuDetail> MenuDetails { get; set; }
    }
}