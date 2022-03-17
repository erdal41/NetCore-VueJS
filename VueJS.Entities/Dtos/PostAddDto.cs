using VueJS.Entities.ComplexTypes;
using VueJS.Entities.Concrete;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VueJS.Entities.Dtos
{
    public class PostAddDto
    {       
        [Required]
        public int UserId { get; set; }

        [Required]
        public string PostName { get; set; }

        [Required]
        public ObjectType PostType { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }

        public string Content { get; set; }

        public string Title { get; set; }

        public PostStatus PostStatus { get; set; }

        public bool CommentStatus { get; set; }

        public int? ParentId { get; set; }

        public int? FeaturedImageId { get; set; }

        public string BottomContent { get; set; }
              
        [Required]
        public bool IsShowPage { get; set; }

        [Required]
        public bool IsShowFeaturedImage { get; set; }
        
        [Required]
        public bool IsShowSubPosts { get; set; }


        public Post Parent { get; set; }
    }
}