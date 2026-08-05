using HRP.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectX.Models
{
    public class Skill
    {
        [Key]
        public int Skill_id { get; set; }
        public string Skill_Name { get; set; }
        public string Skill_Category { get; set; }

     
        public List<UserSkill> UserSkills { get; set; }

        public List<JobPostingSkill> JobPostingSkills { get; set; }



    }
}
