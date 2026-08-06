using HRP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ProjectX.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InterviewsController : ControllerBase
{
    private readonly ProjectContext _context;

    public InterviewsController(ProjectContext context)
    {
        _context = context;
    }

    // Case 1: POST
    // Schedule a new interview
    [HttpPost]
    public async Task<IActionResult> CreateInterview(
        CreateInterviewRequest request)
    {
        var applicationExists = await _context.Applications
            .AnyAsync(a => a.ApplicationID == request.ApplicationID);

        if (!applicationExists)
        {
            return BadRequest("The application does not exist.");
        }

        if (request.InterviewDate <= DateTime.Now)
        {
            return BadRequest("The interview date must be in the future.");
        }

        var interview = new Interview
        {
            InterviewDate = request.InterviewDate,
            InterviewType = request.InterviewType,
            InterviewStage = request.InterviewStage,
            Result_Offer = null,
            ApplicationID = request.ApplicationID
        };

        _context.Interviews.Add(interview);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetInterviewById),
            new { id = interview.InterviewID },
            interview);
    }

    // Case 2: PUT
    // Reschedule an interview
    [HttpPut("{id}/reschedule")]
    public async Task<IActionResult> RescheduleInterview(
        int id,
        RescheduleInterviewRequest request)
    {
        var interview = await _context.Interviews.FindAsync(id);

        if (interview == null)
        {
            return NotFound("Interview not found.");
        }

        if (request.InterviewDate <= DateTime.Now)
        {
            return BadRequest("The interview date must be in the future.");
        }

        interview.InterviewDate = request.InterviewDate;
        interview.InterviewType = request.InterviewType;

        await _context.SaveChangesAsync();

        return Ok(interview);
    }

    // Case 3: PATCH
    // Update the interview stage and result
    [HttpPatch("{id}/result")]
    public async Task<IActionResult> UpdateInterviewResult(
        int id,
        UpdateInterviewResultRequest request)
    {
        var interview = await _context.Interviews.FindAsync(id);

        if (interview == null)
        {
            return NotFound("Interview not found.");
        }

        interview.InterviewStage = request.InterviewStage;
        interview.Result_Offer = request.Result_Offer;

        await _context.SaveChangesAsync();

        return Ok(interview);
    }

    // Case 4: DELETE
    // Delete an interview
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteInterview(int id)
    {
        var interview = await _context.Interviews.FindAsync(id);

        if (interview == null)
        {
            return NotFound("Interview not found.");
        }

        _context.Interviews.Remove(interview);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // Case 5: GET List
    // Get all interviews with Application details
    [HttpGet]
    public async Task<IActionResult> GetAllInterviews()
    {
        var interviews = await _context.Interviews
            .AsNoTracking()
            .Include(i => i.Application)
            .Select(i => new
            {
                i.InterviewID,
                i.InterviewDate,
                i.InterviewType,
                i.InterviewStage,
                i.Result_Offer,
                i.ApplicationID,

                Application = new
                {
                    i.Application.ApplicationID,
                    i.Application.AppliedAt,
                    i.Application.ApplicationStatus,
                    i.Application.JobPostingID
                }
            })
            .ToListAsync();

        return Ok(interviews);
    }

    // Case 6: GET Find
    // Find one interview by ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetInterviewById(int id)
    {
        var interview = await _context.Interviews
            .AsNoTracking()
            .Include(i => i.Application)
            .Where(i => i.InterviewID == id)
            .Select(i => new
            {
                i.InterviewID,
                i.InterviewDate,
                i.InterviewType,
                i.InterviewStage,
                i.Result_Offer,
                i.ApplicationID,

                Application = new
                {
                    i.Application.ApplicationID,
                    i.Application.AppliedAt,
                    i.Application.ApplicationStatus,
                    i.Application.JobPostingID
                }
            })
            .FirstOrDefaultAsync();

        if (interview == null)
        {
            return NotFound("Interview not found.");
        }

        return Ok(interview);
    }

