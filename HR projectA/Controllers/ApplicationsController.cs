using System.Security.Claims;
using HRP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectX.DTOs;
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
    [Authorize (Roles = "Candidate")]
    [HttpPost("Add Application")]
    public IActionResult ADD_Application(Application_DTO A)
    {   
        int userD= int.Parse( User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        
        Context.Applications.Add(new Application
        {
            AppliedAt = A.AppliedAt,
            UserId = userD,
            JobPostingID = A.JobPostingID,
            ApplicationStatus = "inactive"
        });
        Context.SaveChanges();
        return Ok($"application Accepted");
    }

    [Authorize(Roles = "Employee")]
    [HttpPatch("Update Application Status")]

    public IActionResult Update_Application_Status(string Status, int userid)
    {
        // int Id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var role = User.FindFirstValue(ClaimTypes.Role)!;
        if (role == "Employee")
        {
            var companyid = User.FindFirst("CompanyId").Value!;
            if (string.IsNullOrEmpty(companyid) || !int.TryParse(companyid, out int employeeCompanyId))
            {
                return Forbid();

            }

            bool isForEmployeeCompany = Context.Applications
                .Any(a => a.UserId == userid && a.JobPosting.CompanyID == int.Parse(companyid));

            if (!isForEmployeeCompany)
            {
                return Forbid("This application does not belong to your company.");
            }
        }

        Application Updated_App = Context.Applications.FirstOrDefault(A => A.UserId == userid);
            if (Updated_App == null)
            {
                return NotFound("such application does not exist");
            }

            Updated_App.ApplicationStatus = Status;
            Context.Applications.Update(Updated_App);
            Context.SaveChanges();
            return Ok(" Application Status updated");

        }
    

    [Authorize(Roles = "Candidate")]
    [HttpPatch("Update Application Job")]
    public IActionResult Update_Application_Job( int JobPostingId)
    {
        int Id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        Application Updated_App = Context.Applications.FirstOrDefault(A => A.UserId == Id);
        if (Updated_App == null)
        {
            return NotFound("application not found");
        }

        Updated_App.JobPostingID = JobPostingId;
        Context.Applications.Update(Updated_App);
        Context.SaveChanges();
        return Ok("Appllied jop is updated");
    }
    [Authorize(Roles = "Candidate,Admin")]
    [HttpDelete("Delete Application")]
    public IActionResult Remove_Application(int Application_Id)
    {
        Application Removed_Application = Context.Applications.FirstOrDefault(A => A.ApplicationID == Application_Id);
        if (Removed_Application == null)
        {
            return NotFound("Application not found");
        }
        // 3. OWNERSHIP CHECK: If the user is a Candidate, ensure they OWN this application
        int loggedInUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        string userRole = User.FindFirstValue(ClaimTypes.Role)!;
        if (userRole == "Candidate" && Removed_Application.UserId != loggedInUserId)
        {
            return Forbid(); // 403 Forbidden: Candidate is trying to delete someone else's application!
        }


        Context.Applications.Remove(Removed_Application);
        Context.SaveChanges();

        return Ok(new { message = "Application deleted/withdrawn successfully." });
    }
    
    [Authorize(Roles = "Admin")]
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

    [Authorize(Roles = "Candidate,Employee,Admin")]
    [HttpGet("GetApplicationInfo/{applicationId}")]
    public IActionResult Getapplication(int applicationId)
    {
        // Single database query with all inclusions
        var appA = Context.Applications
            .Include(a => a.JobPosting)
            .Include(a => a.Interviews)
            .Include(a => a.Offer)
            .FirstOrDefault(a => a.ApplicationID == applicationId);

        if (appA == null)
        {
            return NotFound("Application not found.");
        }

        int loggedInUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        string userRole = User.FindFirstValue(ClaimTypes.Role)!;

        // 1. Candidate ownership check
        if (userRole == "Candidate" && appA.UserId != loggedInUserId)
        {
            return Forbid();
        }

        // 2. Employee/Employer company ownership check
        if (userRole == "Employee")
        {
            var companyIdClaim = User.FindFirst("CompanyId")?.Value;

            if (string.IsNullOrEmpty(companyIdClaim) || !int.TryParse(companyIdClaim, out int employerCompanyId))
            {
                return Forbid();
            }

            // Direct in-memory check against the requested application's company ID
            if (appA.JobPosting?.CompanyID != employerCompanyId)
            {
                return Forbid(); // Application belongs to another company
            }
            
        }

        return Ok(appA);
    }


    [Authorize(Roles = "Admin")]
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
    [Authorize(Roles = "Admin")]
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