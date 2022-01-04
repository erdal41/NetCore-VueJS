using VueJS.Entities.Dtos;

namespace VueJS.Mvc.Areas.Admin.Models
{
    public class PostTermViewModel
    {
        public PostTermAddDto PostTermAddDto { get; set; }
        public PostTermDto PostTermDto { get; set; }
        public int PostId { get; set; }
        public int TermId { get; set; }
    }
}