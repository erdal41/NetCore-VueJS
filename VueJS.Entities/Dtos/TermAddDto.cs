using VueJS.Entities.ComplexTypes;
using VueJS.Entities.Concrete;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VueJS.Entities.Dtos
{
    public class TermAddDto
    {
        [DisplayName("Kısa İsim")]
        public string Slug { get; set; }

        [DisplayName("İsim")]
        [Required(ErrorMessage = "{0} alanı boş geçilemez.")]
        public string Name { get; set; }

        [DisplayName("Açıklama")]
        [MinLength(0, ErrorMessage = "{0} alanı {1} karakterden küçük olmamalıdır.")]
        [MaxLength(500, ErrorMessage = "{0} alanı {1} karakterden büyük olmamalıdır.")]
        public string Description { get; set; }

        [DisplayName("Ebeveyn Kategori")]
        public int? ParentId { get; set; }
        public ObjectType TermType { get; set; }

        public Term Parent { get; set; }
    }
}