using VueJS.Entities.Dtos;

namespace VueJS.Mvc.Areas.Web.Models
{
    public class PostDetailViewModel
    {
        public UrlRedirectListDto UrlRedirectListDto { get; set; } 
        public PostDto PostDto { get; set; }
        public PostListDto PostListDto { get; set; }
        public GeneralSettingDto GeneralSettingDto { get; set; }
        public SeoGeneralSettingDto SeoGeneralSettingDto { get; set; }
        public SeoObjectSettingDto SeoObjectSettingDto { get; set; }
        //public ArticleDetailRightSideBarViewModel ArticleDetailRightSideBarViewModel { get; set; }
        public string SiteKey { get; set; }
    }
}