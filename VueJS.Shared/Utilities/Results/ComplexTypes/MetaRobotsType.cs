using System.ComponentModel.DataAnnotations;

namespace VueJS.Entities.ComplexTypes
{
    public enum MetaRobotsType
    {
        [Display(Name = "Varsayılan")]
        Default = 0,
        [Display(Name = "Sayfadaki resimleri indexleme")]
        ImageNoIndex = 1,
        [Display(Name = "Bu sayfayı önbelleğe alma")]
        NoArchive = 2,
        [Display(Name = "Bu sayfaki özel snippetleri devre dışı bırak(Örn; video, resim gibi)")]
        NoSnippet = 3,
    }
}