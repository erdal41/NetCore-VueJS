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

        [DisplayName("Sitenin bir kuruluşu mu yoksa bir kişiyi mi temsil ettiğini seçin.")]
        public SeoType SeoType { get; set; }

        [DisplayName("Kuruluş Adı")]
        public string SiteName { get; set; }

        [DisplayName("Resim")]
        public int? SiteMainImageId { get; set; }

        [DisplayName("Düzenlenme Tarihi")]
        [DataType(DataType.DateTime)]
        public DateTime ModifiedDate { get; set; }

        [DisplayName("Google Doğrulama Kodu")]
        [MaxLength(100, ErrorMessage = "{0} alanı {1} karakterden büyük olmamalıdır.")]
        public string GoogleVerificationCode { get; set; }

        [DisplayName("Yandex Doğrulama Kodu")]
        [MaxLength(100, ErrorMessage = "{0} alanı {1} karakterden büyük olmamalıdır.")]
        public string YandexVerificationCode { get; set; }

        [DisplayName("Bing Doğrulama Kodu")]
        [MaxLength(100, ErrorMessage = "{0} alanı {1} karakterden büyük olmamalıdır.")]
        public string BingVerificationCode { get; set; }

        [DisplayName("Baidu Doğrulama Kodu")]
        [MaxLength(100, ErrorMessage = "{0} alanı {1} karakterden büyük olmamalıdır.")]
        public string BaiduVerificationCode { get; set; }

        [DisplayName("XML site haritaları")]
        [Required]
        public bool IsActiveSitemap { get; set; }

        [DisplayName("Robots.txt dosyası")]
        [Required]
        public bool IsActiveRobotsTxt { get; set; }

        [DisplayName("Makaleler arama sonuçlarında gösterilsin mi?")]
        [Required]
        public bool IsActiveArticleSearchEngineIndex { get; set; }

        [DisplayName("Makaleler için SEO ayarlarını göster")]
        [Required]
        public bool IsActiveArticleSeoSetting { get; set; }

        [DisplayName("Varsayılan SEO başlığı")]
        [MaxLength(100, ErrorMessage = "{0} alanı {1} karakterden büyük olmamalıdır.")]
        public string ArticleDefaultSeoTitle { get; set; }

        [DisplayName("Varsayılan meta açıklaması")]
        [MaxLength(150, ErrorMessage = "{0} alanı {1} karakterden büyük olmamalıdır.")]
        public string ArticleDefaultSeoDescription { get; set; }

        [DisplayName("Sayfalar arama sonuçlarında gösterilsin mi?")]
        [Required]
        public bool IsActivePageSearchEngineIndex { get; set; }

        [DisplayName("Sayfalar için SEO ayarlarını göster")]
        [Required]
        public bool IsActivePageSeoSetting { get; set; }

        [DisplayName("Varsayılan SEO başlığı")]
        [MaxLength(100, ErrorMessage = "{0} alanı {1} karakterden büyük olmamalıdır.")]
        public string PageDefaultSeoTitle { get; set; }

        [DisplayName("Varsayılan meta açıklaması")]
        [MaxLength(150, ErrorMessage = "{0} alanı {1} karakterden büyük olmamalıdır.")]
        public string PageDefaultSeoDescription { get; set; }

        [DisplayName("Kategoriler arama sonuçlarında gösterilsin mi?")]
        [Required]
        public bool IsActiveCategorySearchEngineIndex { get; set; }

        [DisplayName("Kategoriler için SEO ayarlarını göster")]
        [Required]
        public bool IsActiveCategorySeoSetting { get; set; }

        [DisplayName("Varsayılan SEO başlığı")]
        [MaxLength(100, ErrorMessage = "{0} alanı {1} karakterden büyük olmamalıdır.")]
        public string CategoryDefaultSeoTitle { get; set; }

        [DisplayName("Varsayılan meta açıklaması")]
        [MaxLength(150, ErrorMessage = "{0} alanı {1} karakterden büyük olmamalıdır.")]
        public string CategoryDefaultSeoDescription { get; set; }

        [DisplayName("Etiketler arama sonuçlarında gösterilsin mi?")]
        [Required]
        public bool IsActiveTagSearchEngineIndex { get; set; }

        [DisplayName("Etiketler için SEO ayarlarını göster")]
        [Required]
        public bool IsActiveTagSeoSetting { get; set; }

        [DisplayName("Varsayılan SEO başlığı")]
        [MaxLength(100, ErrorMessage = "{0} alanı {1} karakterden büyük olmamalıdır.")]
        public string TagDefaultSeoTitle { get; set; }

        [DisplayName("Varsayılan meta açıklaması")]
        [MaxLength(150, ErrorMessage = "{0} alanı {1} karakterden büyük olmamalıdır.")]
        public string TagDefaultSeoDescription { get; set; }

        [DisplayName("Facebook Sayfa Linki")]
        [MaxLength(100, ErrorMessage = "{0} alanı {1} karakterden büyük olmamalıdır.")]
        public string FacebookPageLink { get; set; }

        [DisplayName("Twitter Kullanıcı Adı")]
        [MaxLength(100, ErrorMessage = "{0} alanı {1} karakterden büyük olmamalıdır.")]
        public string TwitterUserName { get; set; }

        [DisplayName("Instagram Profil Linki")]
        [MaxLength(100, ErrorMessage = "{0} alanı {1} karakterden büyük olmamalıdır.")]
        public string InstagramProfilLink { get; set; }

        [DisplayName("LinkedIn Profil Linki")]
        [MaxLength(100, ErrorMessage = "{0} alanı {1} karakterden büyük olmamalıdır.")]
        public string LinkedInProfilLink { get; set; }

        [DisplayName("Pinterest Profil Linki")]
        [MaxLength(100, ErrorMessage = "{0} alanı {1} karakterden büyük olmamalıdır.")]
        public string PinterestProfilLink { get; set; }

        [DisplayName("Youtube Kanal Linki")]
        [MaxLength(200, ErrorMessage = "{0} alanı {1} karakterden büyük olmamalıdır.")]
        public string YoutubeChannelLink { get; set; }

        [DisplayName("Open Graph Meta verisi")]
        [Required]
        public bool IsActiveOpenGraph { get; set; }

        [DisplayName("Varsayılan Görsel")]
        public int? OpenGraphImageId { get; set; }

        [DisplayName("Twitter kartı meta verisi")]
        [Required]
        public bool IsActiveTwitterOpenGraph { get; set; }

        [DisplayName("Twitter kart tipi")]
        [Required]
        public TwitterCardType TwitterCardType { get; set; }

        [DisplayName("Pinterest Onay Kodu")]
        [MaxLength(100, ErrorMessage = "{0} alanı {1} karakterden büyük olmamalıdır.")]
        public string PinterestConfirmationCode { get; set; }

        [Required]
        public int UserId { get; set; }

        public Upload SiteMainImage { get; set; }
        public Upload OpenGraphImage { get; set; }
    }
}