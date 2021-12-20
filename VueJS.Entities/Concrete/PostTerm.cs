using VueJS.Entities.ComplexTypes;
using VueJS.Shared.Entities.Abstract;
using System.ComponentModel.DataAnnotations;

namespace VueJS.Entities.Concrete
{
    public class PostTerm : IEntity
    {
        [Key]
        public int Id { get; set; }
        public int PostId { get; set; }
        public int TermId { get; set; }
        public SubObjectType TermType { get; set; }
                
        public Post Post { get; set; }
        public Term Term { get; set; }
    }
}