using VueJS.Shared.Entities.Abstract;
using System;
using System.ComponentModel.DataAnnotations;

namespace VueJS.Entities.Concrete
{
    public class UrlRedirect :  IEntity
    {
        [Key]
        public int Id { get; set; }
        public string OldUrl { get; set; }
        public string NewUrl { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int UserId { get; set; }

        public User User { get; set; }
    }
}