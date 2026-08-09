using HRP.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json.Serialization;

namespace ProjectX.Models
{
    public class Skill
    {
       
        [Key]
        [JsonIgnore]
        public int Skill_id { get; set; }
        public string Skill_Name { get; set; }
        public string Skill_Category { get; set; }

     [JsonIgnore]
        public List<UserSkill>?UserSkills { get; set; }
[JsonIgnore]
        public List<JobPostingSkill>? JobPostingSkills { get; set; }



    }
}
