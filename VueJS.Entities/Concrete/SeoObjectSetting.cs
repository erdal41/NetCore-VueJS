using VueJS.Entities.ComplexTypes;
using VueJS.Shared.Entities.Abstract;
using System;
using System.ComponentModel.DataAnnotations;

namespace VueJS.Entities.Concrete
{
    public class SeoObjectSetting : IEntity
    {
        public int Id { get; set; }
        public string Permalink { get; set; }
        public int ObjectId { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
        public string SeoTitle { get; set; }
        public string SeoDescription { get; set; }
        public string CanonicalUrl { get; set; }
        public string FocusKeyword { get; set; }
        public bool IsRobotsNoIndex { get; set; } = false;
        public bool IsRobotsNoFollow { get; set; } = false;
        public bool IsRobotsNoArchive { get; set; } = false;
        public bool IsRobotsNoImageIndex { get; set; } = false;
        public bool IsRobotsNoSnippet { get; set; } = false;
       public int? OpenGraphImageId { get; set; }
        public string OpenGraphTitle { get; set; }
        public string OpenGraphDescription { get; set; }
        public int? TwitterImageId { get; set; }
        public string TwitterTitle { get; set; }
        public string TwitterDescription { get; set; }
        public SchemaPageType SchemaPageType { get; set; } = SchemaPageType.Default;
        public SchemaArticleType SchemaArticleType { get; set; } = SchemaArticleType.Default;
        public ObjectType ObjectType { get; set; }
        public SubObjectType SubObjectType { get; set; }

        public User User { get; set; }
        public Upload OpenGraphImage { get; set; }
        public Upload TwitterImage { get; set; }
    }
}