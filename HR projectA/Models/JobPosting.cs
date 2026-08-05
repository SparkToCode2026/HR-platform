using System.ComponentModel.DataAnnotations.Schema;
using ProjectX.Models;

namespace HRP.Models;

public class JobPosting
{
    public int JobPostingID { get; set; }
    public string Title { get; set; } = string.Empty;
    public string AcademicDegree { get; set; } = string.Empty;
    public int PositionsAvailable { get; set; }
    public DateTime JobDeadline { get; set; }
    [ForeignKey("Department")]
    public int DepartmentID { get; set; }
    [ForeignKey("JobCategory")]
    public int JobCategoryID { get; set; }
    [ForeignKey("Company")]
    public int CompanyID { get; set; }

    public Department? Department { get; set; }
    public JopCategory? JobCategory { get; set; }
    public company? Company { get; set; }
    public List<Application> Applications { get; set; } = new();
    public List<JobPostingSkill> JobPostingSkills { get; set; }





}
