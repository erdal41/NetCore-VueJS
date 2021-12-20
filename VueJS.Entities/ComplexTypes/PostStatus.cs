using System.ComponentModel.DataAnnotations;

namespace VueJS.Entities.ComplexTypes
{
    public enum PostStatus
    {
        [Display(Name = "Yayında")]
        publish = 0,
        [Display(Name = "Taslak")]
        draft = 1,
        [Display(Name = "Çöp")]
        trash = 2,
    }
}