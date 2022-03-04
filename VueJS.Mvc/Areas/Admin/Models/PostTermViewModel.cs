using VueJS.Entities.Dtos;
using VueJS.Shared.Utilities.Results.Abstract;

namespace VueJS.Mvc.Areas.Admin.Models
{
    public class PostTermViewModel
    {
        public IDataResult<PostTermDto> PostTermDto { get; set; }
    }
}