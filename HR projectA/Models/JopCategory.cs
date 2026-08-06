using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using HRP.Models;

namespace ProjectX.Models;

public class JopCategory
{
    [Key]
    [JsonIgnore]
    public int JopCategoryid { get; set; }
    public String JopCategoryName { get; set; }
    public string JopCategoryDesc { get; set; }
    
    [JsonIgnore]
   public List<JobPosting> JopPosting {get; set; } // navigation property
}