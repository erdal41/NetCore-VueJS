using VueJS.Shared.Entities.Abstract;
using System;
using System.ComponentModel.DataAnnotations;

namespace VueJS.Entities.Concrete
{
    public class UrlRedirect :  IEntity
    {
        public int Id { get; set; }
        public string OldUrl { get; set; }
        public string NewUrl { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
        public int UserId { get; set; }

        public User User { get; set; }
    }
}