using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectX.Models;

namespace ProjectX.Controllers;

[ApiController]
[Route("[controller]")]
public class JopCategoryController : ControllerBase
{
    private readonly ProjectContext Context;
    
    public JopCategoryController(ProjectContext _context)
    {
        Context = _context;
    }

    [HttpPost("add-new-category")]
    public IActionResult ADD_NewCategory(JopCategory C)
    {
        Context.JopCategories.Add(C);
        Context.SaveChanges();
        return Ok("New category added successfully.");
    }

    [HttpPatch("update-category-name")]
    public IActionResult update_CategoryName(int id, string Name)
    {
        JopCategory Category = Context.JopCategories.FirstOrDefault(C => C.JopCategoryid == id);
        
        // Check if category exists
        if (Category == null)
        {
            return NotFound($"Category with ID {id} was not found.");
        }

        Category.JopCategoryName = Name;
        Context.JopCategories.Update(Category);
        Context.SaveChanges();
        return Ok($"Category ID {id} updated correctly to name: {Name}");
    }

    [HttpPatch("update-category-status")]
    public IActionResult UpdateStatus(int id, string Status)
    {
        JopCategory Category = Context.JopCategories.FirstOrDefault(C => C.JopCategoryid == id);
        
        // Check if category exists
        if (Category == null)
        {
            return NotFound($"Category with ID {id} was not found.");
        }

        Category.CategorysTATUS = Status;
        Context.JopCategories.Update(Category);
        Context.SaveChanges();
        return Ok($"Category status updated correctly to: {Status}");
    }

    [HttpDelete("delete-category")]
    public IActionResult DeleteCategory(string Name)
    {
        JopCategory Category = Context.JopCategories.FirstOrDefault(C => C.JopCategoryName == Name);
        
        // Check if category exists
        if (Category == null)
        {
            return NotFound($"Category with name '{Name}' was not found.");
        }

        Context.JopCategories.Remove(Category);
        Context.SaveChanges();
        return Ok($"'{Category.JopCategoryName}' was removed successfully.");
    }

    [HttpGet("view-all-categories")]
    public IActionResult ViewAllCategories()
    {
        var Categories = Context.JopCategories.Select(c => new
        {
            CategoryName = c.JopCategoryName,
            TotalJobsAvailable = c.JopPosting.Sum(jp => jp.PositionsAvailable)
        }).ToList();

        if (!Categories.Any())
        {
            return NotFound("No categories found.");
        }

        return Ok(Categories);
    }

    [HttpGet("view-categories-detail")]
    public IActionResult ViewCategoryDetails()
    {
        List<JopCategory> jopCategories = Context.JopCategories.ToList();
        
        if (!jopCategories.Any())
        {
            return NotFound("No category details found.");
        }

        return Ok(jopCategories);
    }

    [HttpGet("filter-categories-by-status")]
    public IActionResult FilterCategories()
    {
        List<JopCategory> jopCategories = Context.JopCategories
            .Where(C => C.CategorysTATUS == "Active")
            .ToList();

        if (!jopCategories.Any())
        {
            return NotFound("No active categories found.");
        }

        return Ok(jopCategories);
    }

    [HttpGet("GetJobPostingsCountByCategory")]
    public IActionResult GetJobPostingsCountByCategory()
    {
        var categoryCounts = Context.JobPostings
            .GroupBy(jp => new { jp.JobCategoryID, jp.JobCategory.JopCategoryName })
            .Select(group => new
            {
                CategoryId = group.Key.JobCategoryID,
                CategoryName = group.Key.JopCategoryName,
                TotalPostings = group.Count()
            })
            .ToList();

        if (!categoryCounts.Any())
        {
            return NotFound("No job posting counts found.");
        }

        return Ok(categoryCounts);
    }
}