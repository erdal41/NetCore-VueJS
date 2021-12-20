using VueJS.Entities.Concrete;
using VueJS.Shared.Entities.Abstract;
using System.Collections.Generic;

namespace VueJS.Entities.Dtos
{
    public class UploadListDto : DtoBase
    {
        public IList<Upload> Uploads { get; set; }
    }
}