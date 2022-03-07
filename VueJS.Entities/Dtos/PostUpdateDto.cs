using VueJS.Entities.ComplexTypes;
using VueJS.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VueJS.Entities.Dtos
{
    public class PostUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public string PostName { get; set; }

        [Required]
        public SubObjectType PostType { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime ModifiedDate { get; set; }

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
        public Upload FeaturedImage { get; set; }
        public List<Post> Parents { get; set; }
        public List<PostTerm> PostTerms { get; set; }
        public ICollection<Post> Children { get; set; }
        public IList<Gallery> Galleries { get; set; }
    }
}