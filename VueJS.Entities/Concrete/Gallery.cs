using VueJS.Shared.Entities.Abstract;
using System.ComponentModel.DataAnnotations;

namespace VueJS.Entities.Concrete
{
    public class Gallery : IEntity
    {
        [Key]
        public int Id { get; set; }
        public int PostId { get; set; }
        public int UploadId { get; set; }
        
        public Post Post { get; set; }
        public Upload Upload { get; set; }
    }
}