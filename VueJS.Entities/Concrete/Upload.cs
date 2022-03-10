using VueJS.Shared.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VueJS.Entities.Concrete
{
    public class Upload :  IEntity
    {
        [Key]
        public int Id { get; set; }
        public string FileName { get; set; }
        public string AltText { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string FileUrl { get; set; }
        public string ContentType { get; set; }
        public long Size { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int UserId { get; set; }

        [InverseProperty("Uploads")]
        public User User { get; set; }
        public GeneralSetting Logo { get; set; }
        public GeneralSetting MobileLogo { get; set; }
        public GeneralSetting Icon { get; set; }
        public SeoGeneralSetting SiteMainSetting { get; set; }
        public SeoGeneralSetting OpenGraphSetting { get; set; }
        public SeoGeneralSetting PageSocialSetting { get; set; }
        public SeoGeneralSetting ArticleSocialSetting { get; set; }
        public SeoGeneralSetting CategorySocialSetting { get; set; }
        public SeoGeneralSetting TagSocialSetting { get; set; }
        public ICollection<Post> Posts { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<Gallery> Galleries { get; set; }
        public ICollection<SeoObjectSetting> OpenGraphImages { get; set; }
        public ICollection<SeoObjectSetting> TwitterImages { get; set; }
    }
}