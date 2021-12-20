using VueJS.Entities.Concrete;
using VueJS.Shared.Entities.Abstract;

namespace VueJS.Entities.Dtos
{
    public class PostDto : DtoBase
    {
        public Post Post { get; set; }
    }
}