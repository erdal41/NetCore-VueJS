using VueJS.Entities.ComplexTypes;
using VueJS.Shared.Entities.Abstract;
using System;
using System.Collections.Generic;

namespace VueJS.Entities.Concrete
{
    public class Comment : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public string Email { get; set; }
        public int? ParentId { get; set; }
        public bool IsPersonalInformationsSharing { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
        public int? UserId { get; set; }
        public CommentStatus CommentStatus { get; set; }
        public int PostId { get; set; }
        
        public Post Post { get; set; }
        public User User { get; set; }
        public Comment Parent { get; set; }
        public List<Comment> Parents { get; set; }
        public ICollection<Comment> Children { get; set; }
    }
}