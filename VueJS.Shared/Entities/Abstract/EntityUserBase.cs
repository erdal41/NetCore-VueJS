using System;

namespace VueJS.Shared.Entities.Abstract
{
    public class EntityUserBase
    {
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string UserId { get; set; }
        public string ModifiedByName { get; set; }
    }
}