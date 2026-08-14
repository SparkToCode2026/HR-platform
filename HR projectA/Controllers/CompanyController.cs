using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectX.DTOs;
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
        [Authorize (Roles = ("Employee"))]
        [HttpPost("Register")]
        public IActionResult Register(CreateCompanyDto dto )
        {
                var Company = new company
                {
                    CompanyName= dto.Name,
                    Email = dto.Email,
                    Phone = dto.Phone,
                    CompanyDescription = dto.Description,
                    Industry = dto.Industry,
                    CompanyWebsite = dto.WebsiteUrl,
                    LocationStreet = dto.Location,
                    
                    IsVerified = false,
                };
            context.Companies.Add(Company);
             context.SaveChanges();
             return Ok(Company);
        }
        [HttpPatch("verify")]
        [Authorize(Roles = "Admin")] 
        public IActionResult VerifyCompany(int id, [FromBody] UpdateCompanyVerificationDto dto)
        {
            var company = context.Companies.FirstOrDefault(c => c.CompanyId == id);
    
            if (company == null)
            {
                return NotFound("Company not found.");
            }

            company.IsVerified = dto.IsVerified;
            context.SaveChanges();

            return Ok(new { message = $"Company verification status updated to {company.IsVerified}." });
        }

        // PUT: Update Company 
        [Authorize (Roles = ("Employee"))]
        [HttpPut("UpdateCompany")]
        public IActionResult UpdateCompany(int id, [FromBody]  CreateCompanyDto dto)
        {
           
            var companyIdClaim = User.FindFirst("CompanyId")?.Value;

            if (string.IsNullOrEmpty(companyIdClaim) || !int.TryParse(companyIdClaim, out int employerCompanyId))
            {
                return Forbid(); 
            }

           
            if (employerCompanyId != id)
            {
                return Forbid(); 
            }

            // 3. Retrieve target company profile
            var c = context.Companies.FirstOrDefault(x => x.CompanyId == id);
            if (c == null)
            {
                return NotFound("Company not found.");
            }
            
            c.CompanyName = dto.Name;
            c.CompanyDescription = dto.Description;
            c.CompanyWebsite = dto.WebsiteUrl;
            c.Industry = dto.Industry;
            c.Phone = dto.Phone;
            c.Email = dto.Email;
            c.LocationStreet = dto.Location;

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
