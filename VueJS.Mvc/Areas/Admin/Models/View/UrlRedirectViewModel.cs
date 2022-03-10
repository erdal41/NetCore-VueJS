using VueJS.Entities.Dtos;
using VueJS.Shared.Utilities.Results.Abstract;

namespace VueJS.Mvc.Areas.Admin.Models.View
{
    public class UrlRedirectViewModel
    {
        public IDataResult<UrlRedirectDto> UrlRedirectDto { get; set; }
        public IDataResult<UrlRedirectListDto> UrlRedirectListDto { get; set; }
        public IDataResult<UrlRedirectUpdateDto> UrlRedirectUpdateDto { get; set; }
    }
}