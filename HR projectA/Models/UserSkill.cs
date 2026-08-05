using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectX.Models
{
    public class UserSkill
    {
        [ForeignKey("skill")]
        public int Skill_id { get; set; }
        public Skill skill { get; set; }


        [ForeignKey("user")]
        public int UserId { get; set; }
        public User user { get; set; }
    }
}
