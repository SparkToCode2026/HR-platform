using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectX.Models;

namespace ProjectX.Controllers
{
    [ApiController]
    [Route("Skill")]
    public class SkillController : ControllerBase
    {
        private ProjectContext context;

        public SkillController(ProjectContext _context)
        {
            context = _context;
        }


        // Case 1 - POST
        [HttpPost("AddSkill")]
        public IActionResult AddSkill(Skill s)
        {
            context.Skills.Add(s);
            context.SaveChanges();

            return Ok(s.Skill_id);
        }

        // Case 2 - PATCH (Update Skill Name)
        [HttpPatch("UpdateSkillName")]
        public IActionResult UpdateSkillName(int id, string newName)
        {
            Skill s = context.Skills.FirstOrDefault(s => s.Skill_id == id);

            if (s == null)
            {
                return NotFound("Skill not found");
            }

            s.Skill_Name = newName;

            context.SaveChanges();

            return Ok();
        }

        // Case 3 - PATCH (Update Skill Category)
        [HttpPatch("UpdateSkillCategory")]
        public IActionResult UpdateSkillCategory(int id, string newCategory)
        {
            Skill s = context.Skills.FirstOrDefault(s => s.Skill_id == id);

            if (s == null)
            {
                return NotFound("Skill not found");
            }

            s.Skill_Category = newCategory;

            context.SaveChanges();

            return Ok();
        }

        // Case 4 - DELETE
        [HttpDelete("RemoveSkill")]
        public IActionResult RemoveSkill(int id)
        {
            Skill s = context.Skills.FirstOrDefault(s => s.Skill_id == id);

            if (s == null)
            {
                return NotFound("Skill not found");
            }

            context.Skills.Remove(s);
            context.SaveChanges();

            return Ok("Removed Successfully");
        }

        // Case 5 - GET ALL (Include Users with there Resume)
        [HttpGet("GetAllSkills")]
        public IActionResult GetAllSkills()
        {
            List<Skill> skills = context.Skills
                                        .Include(s => s.UserSkills)
                                        .ThenInclude(us => us.user)
                                        .ThenInclude(u => u.Resume)
                                        .ToList();

            return Ok(skills);
        }

        // Case 6 - GET BY ID
        [HttpGet("GetSkill")]
        public IActionResult GetSkill(int id)
        {
            Skill s = context.Skills
                             .Include(s => s.UserSkills)
                             .ThenInclude(us => us.user)
                             .FirstOrDefault(s => s.Skill_id == id);

            if (s == null)
            {
                return NotFound("Skill not found");
            }

            return Ok(s);
        }

        // Case 7 - GET FILTER
        [HttpGet("GetByCategory")]
        public IActionResult GetByCategory(string category)
        {
            List<Skill> skills = context.Skills
                                        .Where(s => s.Skill_Category.Contains(category))
                                        .ToList();

            return Ok(skills);
        }

        // Case 8 - GET AGGREGATE
        [HttpGet("CountUsers")]
        public IActionResult CountUsers()
        {
            var result = context.Skills
                                .Select(s => new
                                {
                                    SkillName = s.Skill_Name,
                                    UsersCount = s.UserSkills.Count
                                })
                                .ToList();

            return Ok(result);
        }



    }
}
