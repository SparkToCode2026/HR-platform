using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProjectX.Models
{
    public class Notification
    {
        [Key]
        [JsonIgnore]
        public int NotificationId { get; set; }

        [Required]
        public string NotificationMessage { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public DateTime DateTime { get; set; }
        
        [Required]
        public string Status { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }

        [ForeignKey("Company")]
        public int CompanyId { get; set; }
        [JsonIgnore]
        public company Company { get; set; }
    }
}

