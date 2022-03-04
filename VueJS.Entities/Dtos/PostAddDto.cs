using VueJS.Entities.ComplexTypes;
using VueJS.Entities.Concrete;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VueJS.Entities.Dtos
{
    public class PostAddDto
    {       
        [Required]
        public int UserId { get; set; }

        [Required]
        public SubObjectType PostType { get; set; }

        [DisplayName("Oluşturulma Tarihi")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }

        [DisplayName("İçerik")]
        public string Content { get; set; }

        [DisplayName("Başlık")]
        [Required(ErrorMessage = "{0} giriniz.")]
        public string Title { get; set; }

        [DisplayName("Durum")]
        public PostStatus PostStatus { get; set; }

        [DisplayName("Yorumlar aktif olsun mu?")]
        public bool CommentStatus { get; set; }

        [DisplayName("Ebeveyn")]
        public int? ParentId { get; set; }

        [DisplayName("Öne Çıkan Görsel")]
        public int? FeaturedImageId { get; set; }

        [DisplayName("Alt İçerik")]
        public string BottomContent { get; set; }
              
        [DisplayName("Ürün, sayfalarda gösterilsin mi?")]
        [Required]
        public bool IsShowPage { get; set; }

        [DisplayName("Resim, ürün sayfasından gösterilsin mi?")]
        [Required]
        public bool IsShowFeaturedImage { get; set; }
        
        [DisplayName("Ürünün alt ürünleri gösterilsin mi?")]
        [Required]
        public bool IsShowSubPosts { get; set; }


        public Post Parent { get; set; }
    }
}