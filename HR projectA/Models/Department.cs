using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectX.Models;

public class Department
{
    public int DepartmentID { get; set; }

    public string DepartmentName { get; set; }
    public String DepartmentDesc { get; set; }
    [ForeignKey("CompanyA")]
    public int CompanyId { get; set; }
    public company CompanyA { get; set; }

}

