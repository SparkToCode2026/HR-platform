using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProjectX.Models
{
    public class Resume
    {
        [Key]
        [JsonIgnore]
        public int Resume_id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Education { get; set; }

        // Foreign Key
        [ForeignKey("User")]
        public int UserId { get; set; }

        // Navigation
        [JsonIgnore]
        public User? _user { get; set; }

    }
}
