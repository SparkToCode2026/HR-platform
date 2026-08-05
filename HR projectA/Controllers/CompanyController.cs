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
    }
}
