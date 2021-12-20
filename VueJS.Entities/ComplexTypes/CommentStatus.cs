using System.ComponentModel.DataAnnotations;

namespace VueJS.Entities.ComplexTypes
{
    public enum CommentStatus
    {
        [Display(Name = "Benim")]
        mine = 0,
        [Display(Name = "Bekleyen")]
        moderated = 1,
        [Display(Name = "Onaylanmış")]
        approved = 2,
        [Display(Name = "Çöp")]
        trash = 3,
    }
}