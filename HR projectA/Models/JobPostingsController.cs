using Microsoft.AspNetCore.Mvc;

namespace HRprojectA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobPostingsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetJobPostings()
        {
            var jobPostings = new[]
            {
                new { Id = 1, Title = "Software Engineer", Department = "IT" },
                new { Id = 2, Title = "HR Specialist", Department = "Human Resources" }
            };

            return Ok(jobPostings);
        }

        [HttpPost]
        public IActionResult CreateJobPosting([FromBody] object newJob)
        {
            return Ok(new { Message = "Job posting added successfully", Data = newJob });
        }
    }
}

