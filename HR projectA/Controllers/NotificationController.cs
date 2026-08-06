using Microsoft.AspNetCore.Mvc;
using ProjectX.Models;

namespace ProjectX.Controllers
{
    [ApiController]
    [Route("Notification")]
    public class NotificationController : ControllerBase
    {
        private ProjectContext context;

        public NotificationController(ProjectContext _context)
        {
            context = _context;
        }
        
        // Case 1 (POST): Create an in-app system notification for a user.
        [HttpPost("AddNotification")]
        public IActionResult AddNotification(Notification n)
        {
            context.Notifications.Add(n);

            context.SaveChanges();

            return Ok(n.NotificationId);
        }
        
        // Case 2 (PUT/PATCH): Update notification content or target URL.
        
        // Case 3 (PUT/PATCH): Mark notification status as "Read" or "Unread".
        
        // Case 4 (DELETE): Clear/delete a notification record.
        
        // Case 5 (GET - List): Fetch notifications including recipient User details.
        
        // Case 6 (GET - Find): Get notification details by NotificationId.
        
        // Case 7 (GET - Filter): Filter notifications to get unread items for a specific user.
        
        // Case 8 (GET - Sort/Aggregate): Count total unread notifications for a user (Count).
    }
}
