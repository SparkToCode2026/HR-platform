using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectX.DTOs;
using ProjectX.Models;


namespace ProjectX.Controllers
{
    [ApiController]
    [Route("Resume")]
    public class ResumeController:ControllerBase
    {
        private ProjectContext context;

        public ResumeController(ProjectContext _context)
        {
            context = _context;
        }


        // Case 1 - POST
        [Authorize (Roles = "Candidate")]
        [HttpPost("AddResume")]
        public IActionResult AddResume(CreateResumeDto dto)
        {
            int loggedInUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            Resume myResume = new Resume();
            myResume.UserId = loggedInUserId;
            myResume.Education = dto.Education;
            myResume.Summary = dto.Summary;
            myResume.Title = dto.Title;
            context.Resumes.Add(myResume);
            context.SaveChanges();

            // 2. Build the resume
            return Ok("Resume successufully added");
        }


        // Case 2 - PATCH (Update Summary)
        [Authorize (Roles = "Candidate")]
        [HttpPatch("UpdateSummary")]
        public IActionResult UpdateSummary( string newSummary)

        {
            int id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!); // we extract the user id from the token
            Resume r = context.Resumes.FirstOrDefault(r => r.UserId == id);

            if (r == null)
            {
                return NotFound("Resume not found");
            }

            r.Summary = newSummary;

            context.SaveChanges();

            return Ok("summery updated   ");
        }


        // Case 3 - PATC (Update Title)
        [Authorize (Roles = "Candidate")]
        [HttpPatch("UpdateTitle")]
        public IActionResult UpdateTitle( string newTitle)
        {
            int id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!); // we extract the user id from the token
            Resume r = context.Resumes.FirstOrDefault(r => r.UserId== id);

            if (r == null)
            {
                return NotFound("Resume not found");
            }

            r.Title = newTitle;

            context.SaveChanges();

            return Ok("resume titel, updated successfully");
        }

        // Case 4 - DELETE
        [Authorize (Roles = "Candidate")]
        [HttpDelete("RemoveResume")]
        public IActionResult RemoveResume()
        {
            int id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!); // we extract the user id from the token
            Resume r = context.Resumes.FirstOrDefault(r => r.UserId == id);

            if (r == null)
            {
                return NotFound("Resume not found");
            }

            context.Resumes.Remove(r);
            context.SaveChanges();

            return Ok("Removed Successfully");
        }

        // Case 5 - GET ALL (Include User)
        [Authorize (Roles = "Admin,Employee")]
        [HttpGet("GetAllResumes")]
        public IActionResult GetAllResumes()
        {
            List<Resume> resumes = context.Resumes
                                          .Include(r => r._user)
                                          .ToList();

            return Ok(resumes);
        }

        // Case 6 - GET BY ID
        [Authorize (Roles = "Admin,Employee")]
        [HttpGet("GetResume")]
        public IActionResult GetResume(int user_id)
        {
            Resume r = context.Resumes
                              .Include(r => r._user)
                              .FirstOrDefault(r => r.UserId == user_id);

            if (r == null)
            {
                return NotFound("Resume not found");
            }

            return Ok(r);
        }

        // Case 7 - GET FILTER
        [Authorize (Roles = "Admin,Employee")]
        [HttpGet("GetByTitle")]
        public IActionResult GetByTitle(string title)
        {
            List<Resume> resumes = context.Resumes
                                          .Where(r => r.Title==title)
                                          .ToList();

            return Ok(resumes);
        }

        // Count total number of resume records
        [Authorize (Roles = "Admin,Employee")]
        [HttpGet("CountResumes")]
        public IActionResult CountResumes()
        {
            int count = context.Resumes.Count();

            return Ok(count);
        }



    }
}
