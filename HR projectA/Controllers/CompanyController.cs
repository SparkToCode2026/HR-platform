using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectX.Models;

namespace ProjectX.Controllers
{

    [ApiController]
    [Route("Company")]
    public class CompanyController:ControllerBase
    {
        private readonly ProjectContext context;

        public CompanyController(ProjectContext _context)
        {
            context = _context;
        }

        // POST: Register Company
        [Authorize (Roles = ("Employee,Admin"))]
        [HttpPost("Register")]
        public IActionResult Register(company c)
        {
            context.Companies.Add(c);
            context.SaveChanges();
            return Ok(c.CompanyId);
        }

        // PUT: Update Company 
        [Authorize (Roles = ("Employee,Admin"))]
        [HttpPut("UpdateCompany")]
        public IActionResult UpdateCompany(int id, company newCompany)
        {
            var c = context.Companies.FirstOrDefault(x => x.CompanyId == id);
            if (c == null) return NotFound();

            c.CompanyName = newCompany.CompanyName;
            c.CompanyDescription = newCompany.CompanyDescription;
            c.CompanyWebsite = newCompany.CompanyWebsite;
            c.Phone = newCompany.Phone;
            c.Email = newCompany.Email;
            c.LocationStreet = newCompany.LocationStreet;
            context.SaveChanges();
            return Ok("Company updated");
        }

        //
        // PUT: Toggle Verification
        [HttpPut("ToggleVerification")]
        public IActionResult ToggleVerification(int id, bool status)
        {
            var c = context.Companies.FirstOrDefault(x => x.CompanyId == id);
            if (c == null) return NotFound();
            c.IsVerified = status;
            context.SaveChanges();
            return Ok("Verification status updated");
        }

        //Delete: Delete Company
        [Authorize (Roles = ("Admin"))]
        [HttpDelete("DeleteCompany")]
        public IActionResult RemoveCompany(int id)
        {
            var c = context.Companies.FirstOrDefault(x => x.CompanyId == id);
            if (c == null) return NotFound();

            context.Companies.Remove(c);
            context.SaveChanges();
            return Ok("Company removed successfully");
        }

        [Authorize (Roles = ("Employee,Admin,Candidate"))]
        // GET: Get All Companies
        [HttpGet("GetAllCompanies")]
        public IActionResult GetAllCompanies()
        {
            var companies = context.Companies.ToList();
            return Ok(companies);
        }

        // GET: Get Company by Id
        [Authorize (Roles = ("Employee,Admin"))]
        [HttpGet("GetCompany")]
        public IActionResult GetCompany(int id)
        {
            var c = context.Companies.FirstOrDefault(x => x.CompanyId == id);
            return Ok(c);
        }

        // GET: Filter Companies
        [Authorize (Roles = ("Employee,Admin,Candidate"))]
        [HttpGet("FilterByIndustry")]
        public IActionResult FilterByIndustry(string industry)
        {
            var companies = context.Companies.Where(x => x.Industry == industry).ToList();
            return Ok(companies);
        }

        // GET: Aggregate JobPostings
        [Authorize (Roles = ("Employee,Admin,Candidate"))]
        [HttpGet("AggregateJobPostings")]
        public IActionResult AggregateJobPostings()
        {
            var result = context.Companies
                .Select(c => new { c.CompanyName, JobCount = c.JobPostings.Count })
                .ToList();
            return Ok(result);
        }

    }
}
