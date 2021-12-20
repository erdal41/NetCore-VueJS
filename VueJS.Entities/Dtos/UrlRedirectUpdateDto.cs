using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VueJS.Entities.Dtos
{
    public class UrlRedirectUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [DisplayName("Eski Url")]
        [Required(ErrorMessage = "{0} boş geçilmemelidir.")]
        public string OldUrl { get; set; }

        [DisplayName("Yeni Url")]
        [Required(ErrorMessage = "{0} boş geçilmemelidir.")]
        public string NewUrl { get; set; }

        [DisplayName("Açıklama")]
        [MinLength(0, ErrorMessage = "{0} alanı {1} karakterden küçük olmamalıdır.")]
        [MaxLength(500, ErrorMessage = "{0} {1} karakterden büyük olmamalıdır.")]
        public string Description { get; set; }
    }
}