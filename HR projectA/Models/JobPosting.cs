using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using ProjectX.Models;

namespace HRP.Models;

public class JobPosting
{
    [JsonIgnore]
    public int JobPostingID { get; set; }
    public string Title { get; set; } = string.Empty;
    public string AcademicDegree { get; set; } = string.Empty;
    public int PositionsAvailable { get; set; }
    public DateTime JobDeadline { get; set; }
  
    [ForeignKey("JobCategory")]
    public int JobCategoryID { get; set; }
    [ForeignKey("Company")]
    public int CompanyID { get; set; }
    [JsonIgnore]
    public JopCategory? JobCategory { get; set; }
    [JsonIgnore]
    public company? Company { get; set; }
    [JsonIgnore]
    public List<Application>?Applications { get; set; } = new();
    [JsonIgnore]
    public List<JobPostingSkill>?JobPostingSkills { get; set; }





}
