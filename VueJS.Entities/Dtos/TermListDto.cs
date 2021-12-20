using VueJS.Entities.Concrete;
using VueJS.Shared.Entities.Abstract;
using System.Collections.Generic;

namespace VueJS.Entities.Dtos
{
    public class TermListDto : DtoBase
    {
        public IList<Term> Terms { get; set; }
    }
}