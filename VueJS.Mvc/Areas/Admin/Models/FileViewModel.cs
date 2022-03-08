using VueJS.Entities.Dtos;
using VueJS.Shared.Utilities.Results.Abstract;

namespace VueJS.Mvc.Areas.Admin.Models
{
    public class FileViewModel
    {
        public IDataResult<FileDto> FileDto { get; set; }
    }
}