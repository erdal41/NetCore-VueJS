using System.ComponentModel.DataAnnotations;

namespace VueJS.Entities.ComplexTypes
{
    public enum SeoType
    {
        [Display(Name = "Kuruluş")]
        organization = 0,
        [Display(Name = "Kişi")]
        person = 1,
    }
}