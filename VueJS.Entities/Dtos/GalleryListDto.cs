using VueJS.Entities.Concrete;
using VueJS.Shared.Entities.Abstract;
using System.Collections.Generic;

namespace VueJS.Entities.Dtos
{
    public class GalleryListDto : DtoResultBase
    {
        public IList<Gallery> Galleries { get; set; }
    }
}