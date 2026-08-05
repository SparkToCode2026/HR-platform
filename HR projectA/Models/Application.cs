
using ProjectX.Models;

namespace HRP.Models;

public class Application
{
    public int ApplicationID { get; set; }
    public DateTime AppliedAt { get; set; }
    public string ApplicationStatus { get; set; } = string.Empty;

    public int JobPostingID { get; set; }

    public JobPosting? JobPosting { get; set; }
    public Interview? Interview { get; set; }
    public Offer? Offer { get; set; }
    private List<Review> _reviews { get; set; }
     
}
