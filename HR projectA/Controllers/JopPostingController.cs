using HRP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectX.Models;

namespace ProjectX.Controllers;

[ApiController]
[Route("JobPostingsController")]
public class JobPostingsController : ControllerBase
{
    private ProjectContext Context;

    public JobPostingsController(ProjectContext _Context)
    {
        Context = _Context;
    }

    [HttpPost("Add Job Posting")]
    public IActionResult Add_JobPosting(JobPosting J)
    {
        // Check foreign key constraints based on mapping schema
        bool companyExists = Context.Companies.Any(c => c.CompanyId == J.CompanyID);
        if (!companyExists)
        {
            return BadRequest($"Company with ID {J.CompanyID} does not exist.");
        }

        bool categoryExists = Context.JopCategories.Any(jc => jc.JopCategoryid == J.JobCategoryID);
        if (!categoryExists)
        {
            return BadRequest($"JobCategory with ID {J.JobCategoryID} does not exist.");
        }

        Context.JobPostings.Add(J);
        Context.SaveChanges();
        return Ok("Job posting added successfully");
    }

    [HttpGet("Get All Job Postings")]
    public IActionResult GetAllJobPostings()
    {
        List<JobPosting> jobPostings = Context.JobPostings
            .Include(j => j.Company)
            .Include(j => j.JobCategory)
            .Include(j => j.Applications)
            .ToList();

        if (jobPostings.Count == 0)
        {
            return NotFound("No job postings found");
        }

        return Ok(jobPostings);
    }

    [HttpGet("Get Job Posting Info")]
    public IActionResult GetJobPosting(int id)
    {
        JobPosting jobPosting = Context.JobPostings
            .Include(j => j.Company)
            .Include(j => j.JobCategory)
            .Include(j => j.Applications)
            .FirstOrDefault(j => j.JobPostingID == id);

        if (jobPosting == null)
        {
            return NotFound("Job posting not found");
        }

        return Ok(jobPosting);
    }

    [HttpPatch("Update Job Title")]
    public IActionResult Update_JobTitle(int Id, string Title)
    {
        JobPosting updatedJob = Context.JobPostings.FirstOrDefault(j => j.JobPostingID == Id);
        if (updatedJob == null)
        {
            return NotFound("Such job posting does not exist");
        }

        updatedJob.Title = Title;
        Context.JobPostings.Update(updatedJob);
        Context.SaveChanges();
        return Ok("updated");
    }

    [HttpDelete("Delete Job Posting")]
    public IActionResult Remove_JobPosting(int Id)
    {
        JobPosting removedJob = Context.JobPostings.FirstOrDefault(j => j.JobPostingID == Id);
        if (removedJob == null)
        {
            return NotFound("Job posting not found");
        }

        Context.JobPostings.Remove(removedJob);
        Context.SaveChanges();
        return Ok("removed successfully");
    }

    [HttpGet("Filter Job Postings By Company")]
    public IActionResult FilterJobPostingsByCompany(int id)
    {
        List<JobPosting> jobPostings = Context.JobPostings
            .Where(j => j.CompanyID == id)
            .Include(j => j.Company)
            .Include(j => j.JobCategory)
            .ToList();

        if (jobPostings.Count == 0)
        {
            return NotFound("No job postings found for this company");
        }

        return Ok(jobPostings);
    }

    [HttpGet("Filter Job Postings By Category")]
    public IActionResult FilterJobPostingsByCategory(int categoryId)
    {
        List<JobPosting> jobPostings = Context.JobPostings
            .Where(j => j.JobCategoryID == categoryId)
            .Include(j => j.Company)
            .Include(j => j.JobCategory)
            .ToList();

        if (jobPostings.Count == 0)
        {
            return NotFound("No job postings found for this category");
        }

        return Ok(jobPostings);
    }

    [HttpGet("Sort Job Postings Alphabetically")]
    public IActionResult SortJobPostings()
    {
        List<JobPosting> jobPostings = Context.JobPostings
            .OrderBy(j => j.Title)
            .ToList();

        if (jobPostings.Count == 0)
        {
            return NotFound("No job postings found");
        }

        return Ok(jobPostings);
    }
}