namespace HRP.Models;

public class JobPosting
{
    public int JobPostingID { get; set; }
    public string Title { get; set; } = string.Empty;
    public string AcademicDegree { get; set; } = string.Empty;
    public int PositionsAvailable { get; set; }
    public DateTime JobDeadline { get; set; }

    public int DepartmentID { get; set; }
    public int JobCategoryID { get; set; }
    public int CompanyID { get; set; }

    public Department? Department { get; set; }
    public JobCategory? JobCategory { get; set; }
    public company? Company { get; set; }
    public List<Application> Applications { get; set; } = new();
    public List<Interview> Interviews { get; set; } = new();
    public List<Offer> Offers { get; set; } = new();
}
