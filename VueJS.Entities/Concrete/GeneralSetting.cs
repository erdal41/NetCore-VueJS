using VueJS.Shared.Entities.Abstract;
using System;

namespace VueJS.Entities.Concrete
{
    public class GeneralSetting : IEntity
    {
        public int Id { get; set; }
        public int? LogoId { get; set; }
        public int? MobileLogoId { get; set; }
        public int? IconId { get; set; }
        public bool IsUseLogoAdminPanel { get; set; }
        public bool IsUseIconAdminPanel { get; set; }
        public bool IsActiveArticleComments { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
        public int UserId { get; set; }
        public bool IsShowArticleDate { get; set; }
        public bool IsShowArticleAuthor { get; set; }
        public bool IsShowArticleCommentCount { get; set; }
        public bool IsAdminCommentApprove { get; set; }

        public User User { get; set; }        
        public Upload Logo { get; set; }
        public Upload MobileLogo { get; set; }
        public Upload Icon { get; set; }
    }
}