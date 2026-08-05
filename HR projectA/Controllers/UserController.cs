using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using ProjectX.Models;

namespace ProjectX.Controllers
{
    [ApiController]
    [Route("User")]
    public class UserController
    {

        private readonly ProjectContext context;
        private readonly IConfiguration config;

        public UserController(ProjectContext _context, IConfiguration _config)
        {
            context = _context;
            config = _config;
        }

        //Post :Register User\Employee\Admin
        [HttpPost]
        [Route("Register")]

        public IActionResult Register(User u)
        {
            u.PasswordHash = BCrypt.Net.BCrypt.HashPassword(u.PasswordHash);
            context.users.Add(u);
            context.SaveChanges();
            return Ok(u.UserId);
        }
        //
        // PUT: Update Profile
        [HttpPut("UpdateProfile")]
        public IActionResult UpdateProfile(int id, User updatedUser)
        {
            var u = context.users.FirstOrDefault(x => x.UserId == id);
            if (u == null) return NotFound();

            u.Username = updatedUser.Username;
            u.Email = updatedUser.Email;
            u.PhoneNumber = updatedUser.PhoneNumber;
            context.SaveChanges();
            return Ok("Profile updated");
        }

    }
}
