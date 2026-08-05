using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectX.Models;

namespace ProjectX.Controllers
{
    [ApiController]
    [Route("Review")]
    public class ReviewController : ControllerBase
    {
        private ProjectContext context;

        public ReviewController(ProjectContext _context)
        {
            context = _context;
        }

        // Case 1 (POST): Submit a company review or interviewer rating.
        [HttpPost("AddReview")]
        public IActionResult AddReview(Review r)
        {
            context.Reviews.Add(r);

            context.SaveChanges();

            return Ok(r.ReviewId);
        }
        
        // Case 2 (PUT/PATCH): Edit existing review text or rating score.
        [HttpPut("UpdateReview")]
        public IActionResult UpdateReview(int id, Review newReview)
        {
            Review r = context.Reviews.FirstOrDefault(r => r.ReviewId == id);
            
            if (r == null)
            {
                return NotFound("Review not found");
            }
            
            r.Comment = newReview.Comment;
            r.Rating = newReview.Rating;
            
            context.SaveChanges();
            
            return Ok();
        }
        
        // Case 3 (PUT/PATCH): Admin approval status update (e.g., mark as "Approved" or "Flagged").
        
        // Case 4 (DELETE): Delete an inappropriate or obsolete review.
        
        // Case 5 (GET - List): Fetch reviews including related Company and reviewer User details.
        
        // Case 6 (GET - Find): Get review by ReviewId.
        
        // Case 7 (GET - Filter): Filter reviews by rating score (e.g., 4+ stars).
        
        // Case 8 (GET - Sort/Aggregate): Calculate average rating score (Average) per company.
    }
}
