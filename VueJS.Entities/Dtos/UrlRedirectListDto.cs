using VueJS.Entities.Concrete;
using VueJS.Shared.Entities.Abstract;
using System.Collections.Generic;

namespace VueJS.Entities.Dtos
{
    public class UrlRedirectListDto : DtoResultBase
    {
        public IList<UrlRedirect> UrlRedirects { get; set; }
    }
}