using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectX.Models
{
    [PrimaryKey(nameof(Skill_id), nameof(UserId))]
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
