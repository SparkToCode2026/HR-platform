using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectX.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }

        [Required]
        public int Rating { get; set; }

        [Required]
        public string Comment { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [ForeignKey("Application")]
        public int ApplicationId { get; set; }
    }
}

