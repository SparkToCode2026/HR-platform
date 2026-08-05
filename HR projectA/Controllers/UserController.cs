using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProjectX.Models;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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

        //Put: Change Password
        [HttpPut("ChangePassword")] public IActionResult ChangePassword(int id, string newPassword) {
            var u = context.users.FirstOrDefault(x => x.UserId == id);
            if (u == null) return NotFound();

            u.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            context.SaveChanges();
            return Ok("Password changed");
        }

        //Delete: Deactivate User
        [HttpDelete("Deactivate")]
        public IActionResult Deactivate(int id)
        {
            var u = context.users.FirstOrDefault(x => x.UserId == id);
            if (u == null) return NotFound();

            u.IsActive = false;
            context.SaveChanges();
            return Ok("User deactivated");
        }

        //Get: Get all users
        [HttpGet("GetAll")]
        public IActionResult GetAllUsers()
        {
            var users = context.users.ToList();
            return Ok(users);
        }

        //Get: Get user by id
        [HttpGet("GetById")] public IActionResult GetUser(int id) {
            var u = context.users.FirstOrDefault(x => x.UserId == id);
            if (u == null) return NotFound();
            return Ok(u);
        }

        //Get:filter users by role
        [HttpGet("FilterByRole")]
        public IActionResult FilterByRole(string role) {
            var users = context.users.Where(x => x.Role == role).ToList();
          
           return Ok(users);
        }


        // GET: Aggregate by Role
        [HttpGet("AggregateByRole")]
        public IActionResult AggregateByRole()
        {
            var result = context.users
                .GroupBy(x => x.Role)
                .Select(g => new { Role = g.Key, Count = g.Count() })
                .ToList();
            return Ok(result);
        }

        //login with JWT 
        [HttpPost("Login")]
        public IActionResult Login(string email, string password)
        {
            var user = context.users.FirstOrDefault(x => x.Email == email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return Unauthorized("Invalid credentials");
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(config["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return Ok(new { Token = tokenString });
        }



    }
