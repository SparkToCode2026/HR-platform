using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public IActionResult AddResume(Resume r)
        {
            context.Resumes.Add(r);
            context.SaveChanges();

            return Ok(r.Resume_id);
        }


        // Case 2 - PATCH (Update Summary)
        [Authorize (Roles = "Candidate")]
        [HttpPatch("UpdateSummary")]
        public IActionResult UpdateSummary(int id, string newSummary)
        {
            Resume r = context.Resumes.FirstOrDefault(r => r.Resume_id == id);

            if (r == null)
            {
                return NotFound("Resume not found");
            }

            r.Summary = newSummary;

            context.SaveChanges();

            return Ok();
        }


        // Case 3 - PATCH (Update Title)
        [Authorize (Roles = "Candidate")]
        [HttpPatch("UpdateTitle")]
        public IActionResult UpdateTitle(int id, string newTitle)
        {
            Resume r = context.Resumes.FirstOrDefault(r => r.Resume_id == id);

            if (r == null)
            {
                return NotFound("Resume not found");
            }

            r.Title = newTitle;

            context.SaveChanges();

            return Ok();
        }

        // Case 4 - DELETE
        [Authorize (Roles = "Candidate")]
        [HttpDelete("RemoveResume")]
        public IActionResult RemoveResume(int id)
        {
            Resume r = context.Resumes.FirstOrDefault(r => r.Resume_id == id);

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
        [Authorize (Roles = "Candidate,Admin,Employee")]
        [HttpGet("GetResume")]
        public IActionResult GetResume(int id)
        {
            Resume r = context.Resumes
                              .Include(r => r._user)
                              .FirstOrDefault(r => r.Resume_id == id);

            if (r == null)
            {
                return NotFound("Resume not found");
            }

            return Ok(r);
        }

        // Case 7 - GET FILTER
        [Authorize (Roles = ",Candidate,Admin,Employee")]
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
