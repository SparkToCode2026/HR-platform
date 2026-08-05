using Microsoft.AspNetCore.Mvc;
using ProjectX.Models;

namespace ProjectX.Controllers
{

    [ApiController]
    [Route("Company")]
    public class CompanyController
    {
        private readonly ProjectContext context;

        public CompanyController(ProjectContext _context)
        {
            context = _context;
        }

        // POST: Register Company
        [HttpPost("Register")]
        public IActionResult Register(company c)
        {
            context.Companies.Add(c);
            context.SaveChanges();
            return Ok(c.CompanyId);
        }

        // PUT: Update Company 
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
        [HttpDelete("DeleteCompany")]
        public IActionResult RemoveCompany(int id)
        {
            var c = context.Companies.FirstOrDefault(x => x.CompanyId == id);
            if (c == null) return NotFound();

            context.Companies.Remove(c);
            context.SaveChanges();
            return Ok("Company removed successfully");
        }

        // 
        // GET: Get All Companies
        [HttpGet("GetAllCompanies")]
        public IActionResult GetAllCompanies()
        {
            var companies = context.Companies.ToList();
            return Ok(companies);
        }

        // GET: Get Company by Id
        [HttpGet("GetCompany")]
        public IActionResult GetCompany(int id)
        {
            var c = context.Companies.FirstOrDefault(x => x.CompanyId == id);
            return Ok(c);
        }


    }
}
