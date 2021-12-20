using System.ComponentModel.DataAnnotations;

namespace VueJS.Entities.ComplexTypes
{
    public enum TwitterCardType
    {
        [Display(Name = "Özet")]
        Summary = 0,
        [Display(Name = "Büyük Görselli Özet")]
        BigImageSummary = 1,
    }
}