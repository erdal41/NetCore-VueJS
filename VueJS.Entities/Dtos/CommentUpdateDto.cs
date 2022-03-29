using System;
using VueJS.Entities.ComplexTypes;

namespace VueJS.Entities.Dtos
{
    public class CommentUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public string Email { get; set; }
        public int? ParentId { get; set; }
        public bool IsPersonalInformationsSharing { get; set; }
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
        public int? UserId { get; set; }
        public CommentStatus CommentStatus { get; set; }
        public int PostId { get; set; }
    }
}