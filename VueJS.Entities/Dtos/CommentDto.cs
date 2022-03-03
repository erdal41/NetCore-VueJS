using VueJS.Entities.Concrete;
using VueJS.Shared.Entities.Abstract;

namespace VueJS.Entities.Dtos
{
    public class CommentDto : DtoBase
    {
        public Comment Comment { get; set; }
    }
}