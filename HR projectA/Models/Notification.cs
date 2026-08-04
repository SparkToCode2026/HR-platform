using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectX.Models
{
    public class Notification
    {
        [Key]
        public int NotificationId { get; set; }

        [Required]
        public string NotificationMessage { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [ForeignKey("Company")]
        public int CompanyId { get; set; }
    }
}

