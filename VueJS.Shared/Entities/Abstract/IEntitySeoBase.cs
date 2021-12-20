using VueJS.Entities.ComplexTypes;
namespace VueJS.Shared.Entities.Abstract
{
    public interface IEntitySeoBase
    {        
        string SeoTitle { get; set; }
        string SeoTags { get; set; }
        string SeoDescription { get; set; }
        bool IsSearchEngineIndex { get; set; }
        bool IsSearchEngineFollowPageLinks { get; set; }
        MetaRobotsType MetaRobotsType { get; set; }
        string CanonicalUrl { get; set; }
        SchemaType SchemaType { get; set; }
        string FacebookImage { get; set; }
        string FacebookTitle { get; set; }
        string FacebookDescription { get; set; }
        string TwitterImage { get; set; }
        string TwitterTitle { get; set; }
        string TwitterDescription { get; set; }
    }
}