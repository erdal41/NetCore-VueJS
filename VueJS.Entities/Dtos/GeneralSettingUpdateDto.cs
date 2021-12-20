using VueJS.Entities.Concrete;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VueJS.Entities.Dtos
{
    public class GeneralSettingUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [DisplayName("Site Logosu")]
        public int? LogoId { get; set; }

        [DisplayName("Site Mobil Logosu")]
        public int? MobileLogoId { get; set; }

        [DisplayName("Site İkonu")]
        public int? IconId { get; set; }

        [DisplayName("Logo Admin Panelinde de Kullanılsın mı?")]
        public bool IsUseLogoAdminPanel { get; set; }

        [DisplayName("İkon Admin Panelinde de Kullanılsın mı?")]
        public bool IsUseIconAdminPanel { get; set; }

        [DisplayName("Tüm yazılarda yorum özelliği aktif olsun mu?")]
        public bool IsActiveArticleComments { get; set; }

        [Required]
        public int UserId { get; set; }

        [DisplayName("Tüm yazılarda yayın tarihi aktif olsun mu?")]
        public bool IsShowArticleDate { get; set; }

        [DisplayName("Tüm yazılarda yazar aktif olsun mu?")]
        public bool IsShowArticleAuthor { get; set; }

        [DisplayName("Tüm yazılarda yorum sayısı aktif olsun mu?")]
        public bool IsShowArticleCommentCount { get; set; }

        [DisplayName("Yönetici yorumu için onay istensin mi?")]
        public bool IsAdminCommentApprove { get; set; }

        public Upload Logo { get; set; }
        public Upload MobileLogo { get; set; }
        public Upload Icon { get; set; }
    }
}