using HRP.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectX.Models
{
    [PrimaryKey(nameof(JobPostingID), nameof(Skill_id))]
    public class JobPostingSkill
    {
        
        [ForeignKey("jobPosting")]
        public int JobPostingID { get; set; }
        public JobPosting jobPosting { get; set; }


        [ForeignKey("skill")]
        public int Skill_id { get; set; }
        public Skill skill { get; set; }
    }
}
