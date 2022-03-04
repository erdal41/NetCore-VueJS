using VueJS.Entities.Dtos;
using VueJS.Shared.Utilities.Results.Abstract;

namespace VueJS.Mvc.Areas.Admin.Models
{
    public class GalleryViewModel
    {
        public IDataResult<GalleryDto> GalleryDto { get; set; }
    }
}