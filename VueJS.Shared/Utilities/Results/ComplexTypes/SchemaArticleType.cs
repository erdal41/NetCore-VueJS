using System.ComponentModel.DataAnnotations;

namespace VueJS.Entities.ComplexTypes
{
    public enum SchemaArticleType
    {
        [Display(Name = "Yazılar için varsayılan(Makale)")]
        Default = 0,
        [Display(Name = "Makale")]
        Article = 1,
        [Display(Name = "Sosyal Medya Gönderimi")]
        SocialMedia = 2,
        [Display(Name = "Haber Makalesi")]
        News = 3,
        [Display(Name = "Reklam Veren İçeriği Makalesi")]
        Advertisement = 4,
        [Display(Name = "Hiciv Makalesi")]
        Satire = 5,
        [Display(Name = "Bilgi Makalesi")]
        Scholarly = 6,
        [Display(Name = "Teknik Makale")]
        Technical = 7,
        [Display(Name = "Rapor")]
        Report = 8,
        [Display(Name = "Hiçbiri")]
        None = 9,
    }
}