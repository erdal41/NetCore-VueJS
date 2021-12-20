using VueJS.Entities.Concrete;
using VueJS.Shared.Entities.Abstract;
using System.Collections.Generic;

namespace VueJS.Entities.Dtos
{
    public class PostTermListDto : DtoResultBase
    {
        public IList<PostTerm> PostTerms { get; set; }
    }
}