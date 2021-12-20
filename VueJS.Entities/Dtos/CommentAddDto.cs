using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VueJS.Entities.Dtos
{
    public class CommentAddDto
    {
        [DisplayName("Ad Soyad")]
        [Required(ErrorMessage = "{0} alanı boş geçilemez.")]
        [MaxLength(50, ErrorMessage = "{0} {1} karakterden büyük olmamalıdır.")]
        [MinLength(3, ErrorMessage = "{0} {1} karakterden az olmamalıdır.")]
        public string Name { get; set; }

        [DisplayName("Yorum")]
        [Required(ErrorMessage = "{0} alanı boş geçilemez.")]
        [MaxLength(1000, ErrorMessage = "{0} {1} karakterden büyük olmamalıdır.")]
        [MinLength(2, ErrorMessage = "{0} {1} karakterden az olmamalıdır.")]
        public string Text { get; set; }

        [DisplayName("E-Posta Adresi")]
        [Required(ErrorMessage = "{0} alanı boş geçilemez.")]
        [MaxLength(100, ErrorMessage = "{0} {1} karakterden büyük olmamalıdır.")]
        [MinLength(10, ErrorMessage = "{0} {1} karakterden az olmamalıdır.")]
        public string Email { get; set; }

        [Required]
        public bool IsPersonalInformationsSharing { get; set; }
        public int? ParentId { get; set; }
        [Required]
        public int PostId { get; set; }
    }
}