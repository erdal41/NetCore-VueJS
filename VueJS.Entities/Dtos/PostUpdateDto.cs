using VueJS.Entities.ComplexTypes;
using VueJS.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VueJS.Entities.Dtos
{
    public class PostUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public string PostName { get; set; }

        [Required]
        public SubObjectType PostType { get; set; }

        [DisplayName("Düzenlenme Tarihi")]
        [DataType(DataType.DateTime)]
        public DateTime ModifiedDate { get; set; }

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

        [DisplayName("Gönderi diğer sayfalarda gösterilsin mi?")]
        [Required]
        public bool IsShowPage { get; set; }

        [DisplayName("Resim, gönderi sayfasından gösterilsin mi?")]
        [Required]
        public bool IsShowFeaturedImage { get; set; }

        [DisplayName("Gönderinin alt sayfaları gösterilsin mi?")]
        [Required]
        public bool IsShowSubPosts { get; set; }


        public Post Parent { get; set; }
        public Upload FeaturedImage { get; set; }
        public List<Post> Parents { get; set; }
        public List<PostTerm> PostTerms { get; set; }
        public ICollection<Post> Children { get; set; }
        public IList<Gallery> Galleries { get; set; }
    }
}