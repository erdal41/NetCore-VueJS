using VueJS.Entities.ComplexTypes;
using VueJS.Shared.Entities.Abstract;

namespace VueJS.Entities.Concrete
{
    public class PostTerm : IEntity
    {
        public int PostId { get; set; }
        public int TermId { get; set; }
        public ObjectType TermType { get; set; }
                
        public Post Post { get; set; }
        public Term Term { get; set; }
    }
}