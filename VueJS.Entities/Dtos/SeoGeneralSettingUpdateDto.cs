using VueJS.Entities.ComplexTypes;
using VueJS.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VueJS.Entities.Dtos
{
    public class SeoGeneralSettingUpdateDto
    {
        [Required]
        public int Id { get; set; }
        public SeoType SeoType { get; set; }
        public string SiteName { get; set; }
        public int? SiteMainImageId { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime ModifiedDate { get; set; }
        public string GoogleVerificationCode { get; set; }
        public string YandexVerificationCode { get; set; }
        public string BingVerificationCode { get; set; }
        public string BaiduVerificationCode { get; set; }

        [Required]
        public bool IsActiveSitemap { get; set; }

        [Required]
        public bool IsActiveRobotsTxt { get; set; }

        [Required]
        public bool IsActiveArticleSearchEngineIndex { get; set; }

        [Required]
        public bool IsActiveArticleSeoSetting { get; set; }
        public string ArticleDefaultSeoTitle { get; set; }
        public string ArticleDefaultSeoDescription { get; set; }
        public SchemaPageType ArticleSchemaPageType { get; set; }
        public SchemaArticleType ArticleSchemaArticleType { get; set; }
        public int? ArticleSocialImageId { get; set; }
        public string ArticleSocialTitle { get; set; }
        public string ArticleSocialDescription { get; set; }

        [Required]
        public bool IsActivePageSearchEngineIndex { get; set; }

        [Required]
        public bool IsActivePageSeoSetting { get; set; }
        public string PageDefaultSeoTitle { get; set; }
        public string PageDefaultSeoDescription { get; set; }
        public SchemaPageType PageSchemaPageType { get; set; }
        public SchemaArticleType PageSchemaArticleType { get; set; }
        public int? PageSocialImageId { get; set; }
        public string PageSocialTitle { get; set; }
        public string PageSocialDescription { get; set; }

        [Required]
        public bool IsActiveCategorySearchEngineIndex { get; set; }

        [Required]
        public bool IsActiveCategorySeoSetting { get; set; }
        public string CategoryDefaultSeoTitle { get; set; }
        public string CategoryDefaultSeoDescription { get; set; }
        public int? CategorySocialImageId { get; set; }
        public string CategorySocialTitle { get; set; }
        public string CategorySocialDescription { get; set; }

        [Required]
        public bool IsActiveTagSearchEngineIndex { get; set; }

        [Required]
        public bool IsActiveTagSeoSetting { get; set; }
        public string TagDefaultSeoTitle { get; set; }
        public string TagDefaultSeoDescription { get; set; }
        public int? TagSocialImageId { get; set; }
        public string TagSocialTitle { get; set; }
        public string TagSocialDescription { get; set; }
        public string FacebookPageLink { get; set; }
        public string TwitterUserName { get; set; }
        public string InstagramProfilLink { get; set; }
        public string LinkedInProfilLink { get; set; }
        public string PinterestProfilLink { get; set; }
        public string YoutubeChannelLink { get; set; }
        public string WikipediaLink { get; set; }

        [Required]
        public bool IsActiveOpenGraph { get; set; }
        public int? OpenGraphImageId { get; set; }

        [Required]
        public bool IsActiveTwitterOpenGraph { get; set; }

        [Required]
        public TwitterCardType TwitterCardType { get; set; }
        public string PinterestConfirmationCode { get; set; }

        [Required]
        public int UserId { get; set; }

        public Upload SiteMainImage { get; set; }
        public Upload OpenGraphImage { get; set; }
        public Upload ArticleSocialImage { get; set; }
        public Upload PageSocialImage { get; set; }
        public Upload CategorySocialImage { get; set; }
        public Upload TagSocialImage { get; set; }
    }
}