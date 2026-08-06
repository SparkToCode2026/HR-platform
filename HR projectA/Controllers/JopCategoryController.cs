using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectX.Models;

namespace ProjectX.Controllers;

public class JopCategoryController : ControllerBase
{
    private ProjectContext Context;

    public JopCategoryController(ProjectContext _context)
    {
        Context = _context;
    }

    public IActionResult ADD_NewCategory(JopCategory C)
    {
        Context.JopCategories.Add(C);
        Context.SaveChanges();
        return Ok("new category is added:");
    }

    public IActionResult update_CategoryName(int id, String Name)
    {
        JopCategory Category = Context.JopCategories.FirstOrDefault(C => C.JopCategoryid == id);
        Category.JopCategoryName = Name;
        Context.JopCategories.Update(Category);
        Context.SaveChanges();
        return Ok("Correctly updated: Name");
    }

    public IActionResult UpdateStatus(int id, string Status)
    {
        JopCategory Category = Context.JopCategories.FirstOrDefault(C => C.JopCategoryid == id);
        Category.CategorysTATUS = Status;
        Context.JopCategories.Update(Category);
        Context.SaveChanges();
        return Ok("Correctly updated : status");
    }

    public IActionResult DeleteCategory(String Name)
    {
        JopCategory Category = Context.JopCategories.FirstOrDefault(C => C.JopCategoryName == Name);
        Context.JopCategories.Remove(Category);
        Context.SaveChanges();
        return Ok($"{Category.JopCategoryName} is removed succesefully");

    }

    public IActionResult ViewAllCategories()
    {
        int JopNumber = 0;
        var Categories = Context.JopCategories.Select(c => new
        {
            CategoryName = c.JopCategoryName,
            TotalJobsAvailable = c.JopPosting.Sum(jp => jp.PositionsAvailable)
        }).ToList();

        return Ok(Categories);
    }

    public IActionResult ViewCategoryDetails()
    {
        List<JopCategory> jopCategories = Context.JopCategories.ToList();
        return Ok(jopCategories);
    }

    public IActionResult FilterCategories()
    {
        List<JopCategory> jopCategories = Context.JopCategories.Where(C => C.CategorysTATUS == "Active").ToList();
        return Ok(jopCategories);

    }
    public IActionResult GetJobPostingsCountByCategory()
    {
        var categoryCounts = Context.JobPostings
            .GroupBy(jp => new { jp.JobCategoryID, jp.JobCategory.JopCategoryName})
            .Select(group => new
            {
                CategoryId = group.Key.JobCategoryID,
                CategoryName = group.Key.JopCategoryName,
                TotalPostings = group.Count()
            })
            .ToList();

        return Ok(categoryCounts);
    }
}