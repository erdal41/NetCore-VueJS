using System.ComponentModel.DataAnnotations;

namespace VueJS.Entities.Dtos
{
    public class PostTermAddDto
    {
        [Required]
        public int PostId { get; set; }

        [Required]
        public int TermId { get; set; }
    }
}