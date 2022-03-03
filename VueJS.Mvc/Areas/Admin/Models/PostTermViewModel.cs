using VueJS.Entities.Dtos;
using VueJS.Shared.Utilities.Results.Abstract;

namespace VueJS.Mvc.Areas.Admin.Models
{
    public class PostTermViewModel
    {
        public PostTermAddDto PostTermAddDto { get; set; }
        public IDataResult<PostTermDto> PostTermDto { get; set; }
        public int PostId { get; set; }
        public int TermId { get; set; }
    }
}