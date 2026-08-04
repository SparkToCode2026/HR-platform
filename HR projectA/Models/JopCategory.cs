using HRP.Models;

namespace ProjectX.Models;

public class JopCategory
{
    public int JopCategoryid { get; set; }
    public String JopCategoryName { get; set; }
    public string JopCategoryDesc { get; set; }
    
    
   public List<JobPosting> JopPosting {get; set; } // navigation property
}