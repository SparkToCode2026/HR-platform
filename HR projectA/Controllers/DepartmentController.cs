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
    public void Update_Department(int Id, String Name)
    {
        Department Updated_depart = Context.Departments.FirstOrDefault(D => D.DepartmentID == Id);
        Updated_depart.DepartmentName = Name;
        Context.Departments.Update(Updated_depart);
        Context.SaveChanges();
    }
    [HttpPatch("Update Department Description")]
    public void Update_department_desc(int id, String Describtion)
    {

        Department Updated_depart = Context.Departments.FirstOrDefault(D => D.DepartmentID == id);
        Updated_depart.DepartmentDesc = Describtion;
        Context.Departments.Update(Updated_depart);
        Context.SaveChanges();
    }
    [HttpDelete("Delete Department")]
    public void Remove_Department(int Id)
    {
        Department Removed_Department = Context.Departments.FirstOrDefault(C => C.DepartmentID == Id);
        Context.Departments.Remove(Removed_Department);
        Context.SaveChanges();

    } // enter the Department id , to see the comapny detalis that belong to

    [HttpGet("Get All Department")]
    
    public IActionResult GetAllDepartments()
    {
        List<Department> departments = Context.Departments
            .Include(d => d.CompanyA)
            .ToList();

        return Ok(departments);
    }

    public IActionResult Getdepartment(int id)
    {
        Department DeptA = Context.Departments.FirstOrDefault(d => d.DepartmentID == id);
        
        return Ok(DeptA);
    }

    public IActionResult FilterDepartments(int id)
    {
        List<Department> DepartmensB = Context.Departments.Where(D => D.CompanyId == id).ToList();
        return Ok(DepartmensB);

    }

    public IActionResult SortDepartment(){
        List<Department> departments = Context.Departments
            .OrderBy(d => d.DepartmentName)
            .ToList();
        return Ok(departments);
    }
    





}