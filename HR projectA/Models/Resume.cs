using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectX.Models
{
    public class Resume
    {
        [Key]
        public int Resume_id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Education { get; set; }

        // Foreign Key
        [ForeignKey("User")]
        public int UserId { get; set; }

        // Navigation
        public User _user { get; set; }

    }
}
