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
    public void ADD_Department(Department D)
    {
        Context.Departments.Add(D);
        Context.SaveChanges();
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
    public IActionResult Getdepartment(int id)
    {
        Department DeptA = Context.Departments.FirstOrDefault(d => d.DepartmentID == id);
        if (DeptA == null)
        {
            return NotFound("Department  not found");
        }

        return Ok(DeptA);
    }
[HttpGet("filter departments by company")]
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