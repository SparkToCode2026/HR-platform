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