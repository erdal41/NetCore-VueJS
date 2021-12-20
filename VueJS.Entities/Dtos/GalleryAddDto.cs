using VueJS.Entities.Concrete;
using System.ComponentModel.DataAnnotations;

namespace VueJS.Entities.Dtos
{
    public class GalleryAddDto
    {
        [Required]
        public int PostId { get; set; }
        [Required]
        public int UploadId { get; set; }

        public Post Post { get; set; }
        public Upload Upload { get; set; }
    }
}