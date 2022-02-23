using System.Collections.Generic;
using VueJS.Entities.Dtos;

namespace VueJS.Mvc.Areas.Admin.Models
{
    public class UploadViewModel
    {
        public IList<UploadDto> UploadDtos { get; set; }
        public UploadDto UploadDto { get; set; }
        public UploadUpdateDto UploadUpdateDto { get; set; }
    }
}