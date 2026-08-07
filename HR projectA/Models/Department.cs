using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProjectX.Models;

public class Department
{    
    [Key]
    [JsonIgnore]
    public int DepartmentID { get; set; }

    public string DepartmentName { get; set; }
    public String DepartmentDesc { get; set; }
    
    [ForeignKey("CompanyA")]
    public int CompanyId { get; set; }
    [JsonIgnore]
    public company CompanyA { get; set; }
    

}

