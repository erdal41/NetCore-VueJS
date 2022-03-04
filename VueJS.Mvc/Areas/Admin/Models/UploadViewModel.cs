using System.Collections.Generic;
using VueJS.Entities.Dtos;
using VueJS.Shared.Utilities.Results.Abstract;

namespace VueJS.Mvc.Areas.Admin.Models
{
    public class UploadViewModel
    {
        public IList<UploadDto> UploadDtos { get; set; }
        public IDataResult<UploadDto> UploadDto { get; set; }
        public IDataResult<UploadUpdateDto> UploadUpdateDto { get; set; }
    }
}