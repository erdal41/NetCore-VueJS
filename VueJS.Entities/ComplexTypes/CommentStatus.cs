using System.ComponentModel.DataAnnotations;

namespace VueJS.Entities.ComplexTypes
{
    public enum CommentStatus
    {
        [Display(Name = "Bekleyen")]
        moderated = 0,
        [Display(Name = "Onaylanmış")]
        approved = 1,
        [Display(Name = "Çöp")]
        trash = 2,
    }
}