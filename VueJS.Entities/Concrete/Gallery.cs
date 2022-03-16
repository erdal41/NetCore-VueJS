using VueJS.Shared.Entities.Abstract;

namespace VueJS.Entities.Concrete
{
    public class Gallery : IEntity
    {
        public int PostId { get; set; }
        public int UploadId { get; set; }
        
        public Post Post { get; set; }
        public Upload Upload { get; set; }
    }
}