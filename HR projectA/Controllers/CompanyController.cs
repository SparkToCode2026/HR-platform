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

    }
}
