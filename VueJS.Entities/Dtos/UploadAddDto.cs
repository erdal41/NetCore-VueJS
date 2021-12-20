using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VueJS.Entities.Dtos
{
    public class UploadAddDto
    {
        [DisplayName("Dosya")]
        [Required(ErrorMessage = "Lütfen, bir {0} seçiniz.")]
        public string FileName { get; set; }

        [DisplayName("Alternatif Metin")]
        [MaxLength(100, ErrorMessage = "{0} {1} karakterden büyük olmamalıdır.")]
        public string AltText { get; set; }

        [DisplayName("Başlık")]
        [MaxLength(100, ErrorMessage = "{0} {1} karakterden büyük olmamalıdır.")]
        public string Title { get; set; }

        [DisplayName("Alt Başlık")]
        [MaxLength(100, ErrorMessage = "{0} {1} karakterden büyük olmamalıdır.")]
        public string SubTitle { get; set; }

        [DisplayName("Açıklama")]
        [MinLength(0, ErrorMessage = "{0} alanı {1} karakterden küçük olmamalıdır.")]
        [MaxLength(500, ErrorMessage = "{0} {1} karakterden büyük olmamalıdır.")]
        public string Description { get; set; }

        [DisplayName("Dosya Türü")]
        [MaxLength(20, ErrorMessage = "{0} {1} karakterden büyük olmamalıdır.")]
        public string ContentType { get; set; }

        [DisplayName("Dosya Boyutu")]
        public long Size { get; set; }

        [DisplayName("Genişlik")]
        public int Width { get; set; }

        [DisplayName("Yükseklik")]
        public int Height { get; set; }
    }
}