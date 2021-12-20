using VueJS.Entities.Concrete;
using VueJS.Shared.Entities.Abstract;
using System.Collections.Generic;

namespace VueJS.Entities.Dtos
{
    public class PostListDto : DtoBase
    {
        public IList<Post> Posts { get; set; }
        public IList<Gallery> Galleries { get; set; }
    }
}