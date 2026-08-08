using HRP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectX.Models;


namespace ProjectX.Controllers;

[ApiController]
[Route("InterviewsController")]
public class InterviewsController : ControllerBase
{
    private ProjectContext Context;
    private readonly EmailSender emailSender;

    public InterviewsController(ProjectContext _Context, EmailSender _emailSender)
    {
        Context = _Context;
        emailSender = _emailSender;
    }

    // Case 1: POST
    [HttpPost("Add Interview")]
    public async Task<IActionResult> ADD_Interview(Interview I)
    {
        if (I.InterviewDate <= DateTime.Now)
        {
            return BadRequest("The interview date must be in the future.");
        }

        Context.Interviews.Add(I);
        Context.SaveChanges();

        // Trigger Domain Email Requirement
        string candidateEmail = "candidate@example.com"; // Fetch candidate's email from DB/Application
        string subject = "Interview Scheduled";
        string body = $"<h3>Interview Details</h3><p>Your interview is scheduled for <b>{I.InterviewDate}</b> via {I.InterviewType}.</p>";

        await emailSender.SendEmailAsync(candidateEmail, subject, body);

        return Ok("Interview scheduled and email sent successfully");
    }

    // Case 2: PUT/PATCH - Reschedule
    [HttpPatch("Reschedule Interview")]
    public IActionResult Reschedule_Interview(int Id, DateTime NewDate, string Type)
    {
        Interview Updated_Interview = Context.Interviews.FirstOrDefault(i => i.InterviewID == Id);
        if (Updated_Interview == null)
        {
            return NotFound("Interview not found");
        }

        if (NewDate <= DateTime.Now)
        {
            return BadRequest("The interview date must be in the future.");
        }

        Updated_Interview.InterviewDate = NewDate;
        Updated_Interview.InterviewType = Type;

        Context.Interviews.Update(Updated_Interview);
        Context.SaveChanges();
        return Ok("Rescheduled successfully");
    }

    // Case 3: PUT/PATCH - Update Stage & Result
    [HttpPatch("Update Interview Result")]
    public IActionResult Update_Interview_Result(int Id, string Stage, string Result)
    {
        Interview Updated_Interview = Context.Interviews.FirstOrDefault(i => i.InterviewID == Id);
        if (Updated_Interview == null)
        {
            return NotFound("Interview not found");
        }

        Updated_Interview.InterviewStage = Stage;
        Updated_Interview.Result_Offer = Result;

        Context.Interviews.Update(Updated_Interview);
        Context.SaveChanges();
        return Ok("Result updated successfully");
    }

    // Case 4: DELETE
    [HttpDelete("Delete Interview")]
    public IActionResult Remove_Interview(int Id)
    {
        Interview Removed_Interview = Context.Interviews.FirstOrDefault(i => i.InterviewID == Id);
        if (Removed_Interview == null)
        {
            return NotFound("Interview not found");
        }

        Context.Interviews.Remove(Removed_Interview);
        Context.SaveChanges();
        return Ok("Removed successfully");
    }

    // Case 5: GET List (with Include)
    [HttpGet("Get All Interviews")]
    public IActionResult GetAllInterviews()
    {
        List<Interview> interviews = Context.Interviews
            .Include(i => i.Application)
            .ToList();

        if (interviews.Count == 0)
        {
            return NotFound("No interviews found");
        }

        return Ok(interviews);
    }

    // Case 6: GET Find
    [HttpGet("Get interview info")]
    public IActionResult GetInterview(int Id)
    {
        Interview interview = Context.Interviews
            .Include(i => i.Application)
            .FirstOrDefault(i => i.InterviewID == Id);

        if (interview == null)
        {
            return NotFound("Interview not found");
        }

        return Ok(interview);
    }

    // Case 7: GET Filter
    [HttpGet("Filter interviews by date")]
    public IActionResult FilterInterviewsByDate(DateTime startDate, DateTime endDate)
    {
        if (startDate > endDate)
        {
            return BadRequest("The start date cannot be after the end date.");
        }

        List<Interview> interviews = Context.Interviews
            .Where(i => i.InterviewDate >= startDate && i.InterviewDate <= endDate)
            .OrderBy(i => i.InterviewDate)
            .ToList();

        if (interviews.Count == 0)
        {
            return NotFound("No interviews found in this date range");
        }

        return Ok(interviews);
    }

    // Case 8: GET Sort
    [HttpGet("Sort upcoming interviews chronologically")]
    public IActionResult SortUpcomingInterviews()
    {
        List<Interview> interviews = Context.Interviews
            .Where(i => i.InterviewDate >= DateTime.Now)
            .OrderBy(i => i.InterviewDate)
            .ToList();

        if (interviews.Count == 0)
        {
            return NotFound("No upcoming interviews found");
        }

        return Ok(interviews);
    }
}