namespace ProjectX.Models
{
    public class Resume
    {
        public int Resume_id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Education { get; set; }

        // Foreign Key
        public int UserId { get; set; }

        // Navigation
        public User User { get; set; }

    }
}
