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
        [HttpPost("AddResume")]
        public IActionResult AddResume(Resume r)
        {
            context.Resumes.Add(r);
            context.SaveChanges();

            return Ok(r.Resume_id);
        }


        // Case 2 - PATCH (Update Summary)
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
        [HttpGet("GetAllResumes")]
        public IActionResult GetAllResumes()
        {
            List<Resume> resumes = context.Resumes
                                          .Include(r => r._user)
                                          .ToList();

            return Ok(resumes);
        }

        // Case 6 - GET BY ID
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
        [HttpGet("GetByTitle")]
        public IActionResult GetByTitle(string title)
        {
            List<Resume> resumes = context.Resumes
                                          .Where(r => r.Title.Contains(title))
                                          .ToList();

            return Ok(resumes);
        }

        // Count total number of resume records
        [HttpGet("CountResumes")]
        public IActionResult CountResumes()
        {
            int count = context.Resumes.Count();

            return Ok(count);
        }



    }
}
