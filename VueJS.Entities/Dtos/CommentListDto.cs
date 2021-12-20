using VueJS.Entities.Concrete;
using VueJS.Shared.Entities.Abstract;
using System.Collections.Generic;

namespace VueJS.Entities.Dtos
{
    public class CommentListDto : DtoBase
    {
        public IList<Comment> Comments { get; set; }
    }
}