using VueJS.Entities.Dtos;

namespace VueJS.Mvc.Areas.Admin.Models
{
    public class UrlRedirectViewModel
    {
        public UrlRedirectAddDto UrlRedirectAddDto { get; set; }
        public UrlRedirectDto UrlRedirectDto { get; set; }

        public UrlRedirectUpdateDto UrlRedirectUpdateDto { get; set; }
        public string UrlRedirectUpdatePartial { get; set; }
    }
}