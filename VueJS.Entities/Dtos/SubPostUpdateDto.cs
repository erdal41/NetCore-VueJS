using VueJS.Entities.Concrete;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VueJS.Entities.Dtos
{
    public class SubPostUpdateDto
    {
        [Required]
        public int Id { get; set; }
        public int? ParentId { get; set; }        
        public List<int> SubPostId { get; set; }        
        public Post Parent { get; set; }
    }
}