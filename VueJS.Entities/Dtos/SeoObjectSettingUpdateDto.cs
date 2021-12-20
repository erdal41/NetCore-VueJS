using VueJS.Entities.ComplexTypes;
using VueJS.Entities.Concrete;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VueJS.Entities.Dtos
{
    public class SeoObjectSettingUpdateDto
    {
        [Required]
        public int Id { get; set; }
        public int? PostId { get; set; }
        public int? TermId { get; set; }

        [Required]
        public int UserId { get; set; }

        [DisplayName("Düzenlenme Tarihi")]
        [DataType(DataType.DateTime)]
        public DateTime ModifiedDate { get; set; }

        [DisplayName("Seo Başlığı")]
        [MaxLength(100, ErrorMessage = "{0} alanı {1} karakterden büyük olmamalıdır.")]
        public string SeoTitle { get; set; }

        [DisplayName("Seo Açıklaması")]
        [MinLength(0, ErrorMessage = "{0} alanı {1} karakterden küçük olmamalıdır.")]
        [MaxLength(150, ErrorMessage = "{0} alanı {1} karakterden büyük olmamalıdır.")]
        public string SeoDescription { get; set; }

        [DisplayName("Seo Etiketi")]
        [MaxLength(70, ErrorMessage = "{0} alanı {1} karakterden büyük olmamalıdır.")]
        public string FocusKeyword { get; set; }

        [DisplayName("Benzer Sayfa Linki")]
        public string CanonicalUrl { get; set; }

        [DisplayName("Arama motorlarının bu sayfayı arama sonuçlarında göstermesini istiyor musunuz?")]
        [Required]
        public bool IsRobotsNoIndex { get; set; }

        [DisplayName("Arama motorları bu sayfa üzerindeki bağlantıları(linkleri) izlemeli mi?")]
        [Required]
        public bool IsRobotsNoFollow { get; set; }

        [DisplayName("Arama motorları bu sayfanın önbelleğini almalı mı?")]
        [Required]
        public bool IsRobotsNoArchive { get; set; }

        [DisplayName("Arama motorları bu sayfa üzerindeki resimleri indexlemeli mi?")]
        [Required]
        public bool IsRobotsNoImageIndex { get; set; }

        [DisplayName("Arama motorları bu sayfa için özel snippetlere izin vermeli mi?")]
        [Required]
        public bool IsRobotsNoSnippet { get; set; }

        [DisplayName("Sosyal Medya Resmi")]
        public int? OpenGraphImageId { get; set; }

        [DisplayName("Facebook Başlığı")]
        public string OpenGraphTitle { get; set; }

        [DisplayName("Facebook Açıklaması")]
        public string OpenGraphDescription { get; set; }

        [DisplayName("Twitter Resmi")]
        public int? TwitterImageId { get; set; }

        [DisplayName("Twitter Başlığı")]
        public string TwitterTitle { get; set; }

        [DisplayName("Twitter Açıklaması")]
        public string TwitterDescription { get; set; }

        [DisplayName("Sayfa Türü")]
        [Required]
        public SchemaType SchemaType { get; set; }

        [Required]
        public ObjectType ObjectType { get; set; }

        [Required]
        public SubObjectType SubObjectType { get; set; }


        public Post Post { get; set; }
        public Term Term { get; set; }
        public User User { get; set; }
        public Upload OpenGraphImage { get; set; }
        public Upload TwitterImage { get; set; }
    }
}