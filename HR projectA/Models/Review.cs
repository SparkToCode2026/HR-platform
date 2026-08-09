using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using HRP.Models;

namespace ProjectX.Models
{
    public class Review
    {
        [Key]
        [JsonIgnore]
        public int ReviewId { get; set; }

        [Required]
        public int Rating { get; set; }

        [Required]
        public string Comment { get; set; }
        
        [Required]
        public string Status { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User? User { get; set; }

        [ForeignKey("Application")]
        
        public int ApplicationId { get; set; }
        [JsonIgnore]
        public Application? Application { get; set; }
    }
}

