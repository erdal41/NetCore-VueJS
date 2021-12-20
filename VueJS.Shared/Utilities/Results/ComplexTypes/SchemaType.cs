using System.ComponentModel.DataAnnotations;

namespace VueJS.Entities.ComplexTypes
{
    public enum SchemaType
    {
        [Display(Name = "Sayfalar için varsayılan(Web sayfası)")]
        Default = 0,
        [Display(Name = "Web Sayfası")]
        WebPage = 1,
        [Display(Name = "Ürün Sayfası")]
        ProductPage = 2,
        [Display(Name = "Hakkında Sayfası")]
        AboutPage = 3,
        [Display(Name = "SSS Sayfası")]
        SSSPage = 4,
        [Display(Name = "Soru-Cevap Sayfası")]
        QuestionAnswerPage = 5,
        [Display(Name = "Profil Sayfası")]
        ProfilPage = 6,
        [Display(Name = "İletişim Sayfası")]
        ContactPage = 7,
        [Display(Name = "Medical Web Sayfası")]
        MedicalWebPage = 8,
        [Display(Name = "Koleksiyon Sayfası")]
        CollectionPage = 9,
        [Display(Name = "Ödeme Sayfası")]
        PaymentPage = 10,
        [Display(Name = "Gayrimenkul Sayfası")]
        RealEstatePage = 11,
        [Display(Name = "Arama Sonuçları Sayfası")]
        SearchResultsPage = 12,
    }
}