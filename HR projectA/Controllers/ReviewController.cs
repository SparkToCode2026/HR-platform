using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectX.Models;
using Microsoft.AspNetCore.Authorization;

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
        [Authorize(Roles = "Admin")] // only administrators should be able to approve/flag reviews
        [HttpPatch("UpdateReviewStatus")]
        public IActionResult UpdateReviewStatus(int id, string status)
        {
            Review r = context.Reviews.FirstOrDefault(r => r.ReviewId == id);

            if (r == null)
            {
                return NotFound("Review not found");
            }

            r.Status = status;

            context.SaveChanges();

            return Ok();
        }
        
        // Case 4 (DELETE): Delete an inappropriate or obsolete review.
        [HttpDelete("DeleteReview")]
        public IActionResult DeleteReview(int id)
        {
            Review r = context.Reviews.FirstOrDefault(r => r.ReviewId == id);

            if (r == null)
            {
                return NotFound("Review not found");
            }

            context.Reviews.Remove(r);
            context.SaveChanges();

            return Ok("Review deleted successfully");
        }
        
        // Case 5 (GET - List): Fetch reviews including related Company and reviewer User details.
        [HttpGet("GetAllReviews")]
        public IActionResult GetAllReviews()
        {
            List<Review> reviews = context.Reviews
                .Include(r => r.User)
                .Include(r => r.Application)
                .ThenInclude(a => a.JobPosting)
                .ThenInclude(j => j.Company)
                .ToList();

            return Ok(reviews);
        }
        
        // Case 6 (GET - Find): Get review by ReviewId.
        [HttpGet("GetReview/{id}")]
        public IActionResult GetReview(int id)
        {
            Review? review = context.Reviews
                .Include(r => r.User)
                .Include(r => r.Application)
                .ThenInclude(a => a.JobPosting)
                .ThenInclude(j => j.Company)
                .FirstOrDefault(r => r.ReviewId == id);

            if (review == null)
            {
                return NotFound("Review not found");
            }

            return Ok(review);
        }
        
        // Case 7 (GET - Filter): Filter reviews by rating score (e.g., 4+ stars).
        [HttpGet("FilterByRating/{rating}")]
        public IActionResult FilterByRating(int rating)
        {
            List<Review> reviews = context.Reviews
                .Include(r => r.User)
                .Include(r => r.Application)
                .ThenInclude(a => a.JobPosting)
                .ThenInclude(j => j.Company)
                .Where(r => r.Rating >= rating)
                .ToList();

            if (reviews.Count == 0)
            {
                return NotFound("No reviews found.");
            }

            return Ok(reviews);
        }
        
        // Case 8 (GET - Sort/Aggregate): Calculate average rating score (Average) per company.
        [HttpGet("AverageRatingPerCompany")]
        public IActionResult AverageRatingPerCompany()
        {
            var result = context.Reviews
                .Include(r => r.Application)
                .ThenInclude(a => a.JobPosting)
                .ThenInclude(j => j.Company)
                .GroupBy(r => r.Application.JobPosting.Company!.CompanyName)
                .Select(g => new
                {
                    Company = g.Key,
                    AverageRating = g.Average(r => r.Rating)
                })
                .ToList();

            return Ok(result);
        }
    }
}
