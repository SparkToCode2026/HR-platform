using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using ProjectX.Models;
using ProjectX.DTOs;
using ProjectX.Services;

namespace ProjectX.Controllers
{
    [ApiController]
    [Route("User")]
    public class UserController : ControllerBase
    {
        private readonly ProjectContext _context;
        private readonly Jwtservice _jwtService;

        public UserController(ProjectContext context, Jwtservice jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        // POST: Register User/Employee/Admin
        [HttpPost("Register")]
        public IActionResult Register([FromBody] RegisterDto dto)
        {
            if (_context.users.Any(u => u.Email == dto.Email))
            {
                return BadRequest(new { message = "Email already registered." });
            }

            var newUser = new User
            {
                Username = dto.Name,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = string.IsNullOrEmpty(dto.Role) ? "Candidate" : dto.Role,
                PhoneNumber = dto.PhoneNumber
            };

            _context.users.Add(newUser);
            _context.SaveChanges();

            return Ok(new { message = "Registration successful!" });
        }

        // POST: Login with JWT (NOW USING Jwtservice)
        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginDto model)
        {
            var user = _context.users.FirstOrDefault(x => x.Email == model.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                return Unauthorized("Invalid credentials");
            }

            // ✅ Use injected Jwtservice to build the properly formatted token
            var tokenString = _jwtService.GenerateToken(user);

            return Ok(new AuthResponseDto
            {
                Token = tokenString,
                Email = user.Email,
                Role = user.Role
            });
        }

        // PUT: Update Profile
        [HttpPut("UpdateProfile")]
        public IActionResult UpdateProfile(int id, User updatedUser)
        {
            var u = _context.users.FirstOrDefault(x => x.UserId == id);
            if (u == null) return NotFound();

            u.Username = updatedUser.Username;
            u.Email = updatedUser.Email;
            u.PhoneNumber = updatedUser.PhoneNumber;
            _context.SaveChanges();
            return Ok("Profile updated");
        }

        // PUT: Change Password
        [HttpPut("ChangePassword")]
        public IActionResult ChangePassword(int id, string newPassword)
        {
            var u = _context.users.FirstOrDefault(x => x.UserId == id);
            if (u == null) return NotFound();

            u.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            _context.SaveChanges();
            return Ok("Password changed");
        }

        // DELETE: Deactivate User
        [HttpDelete("Deactivate")]
        public IActionResult Deactivate(int id)
        {
            var u = _context.users.FirstOrDefault(x => x.UserId == id);
            if (u == null) return NotFound();

            u.IsActive = false;
            _context.SaveChanges();
            return Ok("User deactivated");
        }

        // GET: Get all users
        [HttpGet("GetAll")]
        public IActionResult GetAllUsers()
        {
            var users = _context.users.ToList();
            return Ok(users);
        }

        // GET: Get user by id
        [HttpGet("GetById")]
        public IActionResult GetUser(int id)
        {
            var u = _context.users.FirstOrDefault(x => x.UserId == id);
            if (u == null) return NotFound();
            return Ok(u);
        }

        // GET: Filter users by role
        [HttpGet("FilterByRole")]
        public IActionResult FilterByRole(string role)
        {
            var users = _context.users.Where(x => x.Role == role).ToList();
            return Ok(users);
        }

        // GET: Aggregate by Role
        [HttpGet("AggregateByRole")]
        public IActionResult AggregateByRole()
        {
            var result = _context.users
                .GroupBy(x => x.Role)
                .Select(g => new { Role = g.Key, Count = g.Count() })
                .ToList();
            return Ok(result);
        }
    }
}