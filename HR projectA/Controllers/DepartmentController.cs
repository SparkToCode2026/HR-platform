using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectX.Models;

namespace ProjectX.Controllers;
[ApiController]
[Route("DepartmentController")]
public class DepartmentController:ControllerBase
{
    private ProjectContext Context;

    public DepartmentController(ProjectContext _Context)
    {
        Context = _Context;
    }

    [HttpPost("Add Department")]
    [Authorize( Roles = "Employee")]
    public IActionResult ADD_Department(Department D)
    {
        int CompanyId_user = int.Parse(User.FindFirst("CompanyId").Value!);
        var DepartmentA = new Department
        {
            DepartmentName = D.DepartmentName,
            DepartmentDesc = D.DepartmentDesc,
            CompanyId = CompanyId_user,
       
        };
        
        Context.Departments.Add(DepartmentA);
        Context.SaveChanges();
        return Ok("Great, department added successfully");
    }
    [HttpPatch("Update Department")]

    public IActionResult Update_Department(int Id, String Name)
    {
        Department Updated_depart = Context.Departments.FirstOrDefault(D => D.DepartmentID == Id);
        if (Updated_depart == null)
        {
            return NotFound(" such department does not exist");
        }
        Updated_depart.DepartmentName = Name;
        Context.Departments.Update(Updated_depart);
        Context.SaveChanges();
        return Ok("updated");
    }
    [HttpPatch("Update Department Description")]
    public IActionResult Update_department_desc(int id, String Describtion)
    {

        Department Updated_depart = Context.Departments.FirstOrDefault(D => D.DepartmentID == id);
        if (Updated_depart== null)
        {
            return NotFound("Notification not found");
        }

        Updated_depart.DepartmentDesc = Describtion;
        Context.Departments.Update(Updated_depart);
        Context.SaveChanges();
        return Ok("updated ");
    }
    [HttpDelete("Delete Department")]
    [Authorize (Roles = "Admin")]
    public IActionResult Remove_Department(int Id)
    {
        Department Removed_Department = Context.Departments.FirstOrDefault(C => C.DepartmentID == Id);
        if (Removed_Department== null)
        {
            return NotFound("Department not found");
        }

        Context.Departments.Remove(Removed_Department);
        Context.SaveChanges();
        return Ok("removed successfully");

    } // enter the Department id , to see the comapny detalis that belong to

    [HttpGet("Get All Department")]
    [Authorize (Roles = "Admin")]
    public IActionResult GetAllDepartments()
    {
        List<Department> departments = Context.Departments
            .Include(d => d.CompanyA)
            .ToList();
        if (departments.Count==0)
        {
            return NotFound("No department found");
        }


        return Ok(departments);
    }

    [HttpGet ("Get departmentinfo")]
    [Authorize (Roles = "Admin,Employee")]
    public IActionResult Getdepartment(int id)
    {
        var role = User.FindFirstValue(ClaimTypes.Role)!;
        Department departmentA = Context.Departments.FirstOrDefault(c => c.DepartmentID == id);
        if (departmentA== null)
        {
            return NotFound("Department  not found");
        }
        var companyid_depart = departmentA.CompanyId;
        
        if (role == "Employee")
        {
            var Companyid_user = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            if (int.Parse(Companyid_user) != companyid_depart)
            {
                return Forbid("invalid output for you");
            }
        }
    

        return Ok(departmentA);
    }
[HttpGet("filter departments by company id")]
[Authorize]
    public IActionResult FilterDepartments(int id)
    {
        List<Department> DepartmensB = Context.Departments.Where(D => D.CompanyId == id).ToList();
        if (DepartmensB.Count == 0)
        {
            return NotFound(" no departments found");
        }
        return Ok(DepartmensB);

    }
[HttpGet("Sort Department alphabatically")]
[Authorize]
    public IActionResult SortDepartment(){
        List<Department> departments = Context.Departments
            .OrderBy(d => d.DepartmentName)
            .ToList();
        if (departments.Count == 0)
        {
            return NotFound(" no departments found");
        }
        return Ok(departments);
    }
    





}