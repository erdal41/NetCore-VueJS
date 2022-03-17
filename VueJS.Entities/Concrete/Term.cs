using System.Collections.Generic;
using VueJS.Entities.ComplexTypes;
using VueJS.Shared.Entities.Abstract;

namespace VueJS.Entities.Concrete
{
    public class Term : IEntity
    {
        public int Id { get; set; }
        public string Slug { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ParentId { get; set; }
        public ObjectType TermType { get; set; }
        public int Count { get; set; }

        public Term Parent { get; set; }
        public List<Term> Parents { get; set; }
        public ICollection<Term> Children { get; set; }
        public ICollection<PostTerm> PostTerms { get; set; }
    }
}