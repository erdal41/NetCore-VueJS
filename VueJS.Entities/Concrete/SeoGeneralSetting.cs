using VueJS.Entities.ComplexTypes;
using VueJS.Shared.Entities.Abstract;
using System;
using System.ComponentModel.DataAnnotations;

namespace VueJS.Entities.Concrete
{
    public class SeoGeneralSetting : IEntity
    {
        [Key]
        public int Id { get; set; }
        public SeoType SeoType { get; set; }
        public string SiteName { get; set; }
        public int? SiteMainImageId { get; set; }
        public string GoogleVerificationCode { get; set; }
        public string YandexVerificationCode { get; set; }
        public string BingVerificationCode { get; set; }
        public string BaiduVerificationCode { get; set; }
        public bool IsActiveSitemap { get; set; }
        public bool IsActiveRobotsTxt { get; set; }
        public bool IsActiveArticleSearchEngineIndex { get; set; }
        public bool IsActiveArticleSeoSetting { get; set; }
        public string ArticleDefaultSeoTitle { get; set; }
        public string ArticleDefaultSeoDescription { get; set; }
        public bool IsActivePageSearchEngineIndex { get; set; }
        public bool IsActivePageSeoSetting { get; set; }
        public string PageDefaultSeoTitle { get; set; }
        public string PageDefaultSeoDescription { get; set; }
        public bool IsActiveCategorySearchEngineIndex { get; set; }
        public bool IsActiveCategorySeoSetting { get; set; }
        public string CategoryDefaultSeoTitle { get; set; }
        public string CategoryDefaultSeoDescription { get; set; }
        public bool IsActiveTagSearchEngineIndex { get; set; }
        public bool IsActiveTagSeoSetting { get; set; }
        public string TagDefaultSeoTitle { get; set; }
        public string TagDefaultSeoDescription { get; set; }
        public string FacebookPageLink { get; set; }
        public string TwitterUserName { get; set; }
        public string InstagramProfilLink { get; set; }
        public string LinkedInProfilLink { get; set; }
        public string PinterestProfilLink { get; set; }
        public string YoutubeChannelLink { get; set; }
        public bool IsActiveOpenGraph { get; set; }
        public int? OpenGraphImageId { get; set; }
        public bool IsActiveTwitterOpenGraph { get; set; }
        public TwitterCardType TwitterCardType { get; set; }
        public string PinterestConfirmationCode { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int UserId { get; set; }

        public User User { get; set; }
        public Upload SiteMainImage { get; set; }
        public Upload OpenGraphImage { get; set; }
    }
}