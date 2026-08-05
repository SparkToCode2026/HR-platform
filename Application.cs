namespace HRP.Models;

public class Application
{
    public int ApplicationID { get; set; }
    public DateTime AppliedAt { get; set; }
    public string ApplicationStatus { get; set; } = string.Empty;

    public int JobPostingID { get; set; }

    public JobPosting? JobPosting { get; set; }
    public List<Interview> Interviews { get; set; } = new();
    public List<Offer> Offers { get; set; } = new();
}
