using HRP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectX.Models;

namespace ProjectX.Controllers;

[ApiController]
[Route("ApplicationsController")]
public class ApplicationsController : ControllerBase
{
    private ProjectContext Context;

    public ApplicationsController(ProjectContext _Context)
    {
        Context = _Context;
    }

    [HttpPost("Add Application")]
    public void ADD_Application(Application A)
    {
        Context.Applications.Add(A);
        Context.SaveChanges();
    }

    [HttpPatch("Update Application Status")]
    public IActionResult Update_Application_Status(int Id, string Status)
    {
        Application Updated_App = Context.Applications.FirstOrDefault(A => A.ApplicationID == Id);
        if (Updated_App == null)
        {
            return NotFound("such application does not exist");
        }

        // Assuming your Application model has a Status property
        Updated_App.ApplicationStatus= Status;
        Context.Applications.Update(Updated_App);
        Context.SaveChanges();
        return Ok("updated");
    }

    [HttpPatch("Update Application Job")]
    public IActionResult Update_Application_Job(int Id, int JobPostingId)
    {
        Application Updated_App = Context.Applications.FirstOrDefault(A => A.ApplicationID == Id);
        if (Updated_App == null)
        {
            return NotFound("application not found");
        }

        Updated_App.JobPostingID = JobPostingId;
        Context.Applications.Update(Updated_App);
        Context.SaveChanges();
        return Ok("updated");
    }

    [HttpDelete("Delete Application")]
    public IActionResult Remove_Application(int Id)
    {
        Application Removed_Application = Context.Applications.FirstOrDefault(A => A.ApplicationID == Id);
        if (Removed_Application == null)
        {
            return NotFound("Application not found");
        }

        Context.Applications.Remove(Removed_Application);
        Context.SaveChanges();
        return Ok("removed successfully");
    }

    [HttpGet("Get All Application")]
    public IActionResult GetAllApplications()
    {
        List<Application> applications = Context.Applications
            .Include(a => a.JobPosting)
            .Include(a => a.Interviews)
            .Include(a => a.Offer)
            .ToList();

        if (applications.Count == 0)
        {
            return NotFound("No application found");
        }

        return Ok(applications);
    }

    [HttpGet("Get applicationinfo")]
    public IActionResult Getapplication(int id)
    {
        Application AppA = Context.Applications
            .Include(a => a.JobPosting)
            .Include(a => a.Interviews)
            .Include(a => a.Offer)
            .FirstOrDefault(a => a.ApplicationID == id);

        if (AppA == null)
        {
            return NotFound("Application not found");
        }

        return Ok(AppA);
    }

    [HttpGet("filter applications by job posting")]
    public IActionResult FilterApplications(int id)
    {
        List<Application> ApplicationsB = Context.Applications
            .Where(A => A.JobPostingID == id)
            .ToList();

        if (ApplicationsB.Count == 0)
        {
            return NotFound("no applications found");
        }

        return Ok(ApplicationsB);
    }

    [HttpGet("Sort Application by date")]
    public IActionResult SortApplication()
    {
        List<Application> applications = Context.Applications
            .OrderByDescending(a => a.ApplicationID)
            .ToList();

        if (applications.Count == 0)
        {
            return NotFound("no applications found");
        }

        return Ok(applications);
    }
}