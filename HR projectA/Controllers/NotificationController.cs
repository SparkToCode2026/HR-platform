using Microsoft.AspNetCore.Mvc;
using ProjectX.Models;
using Microsoft.EntityFrameworkCore;

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
        [HttpPatch("UpdateNotification")]
        public IActionResult UpdateNotification(int id, Notification newNotification)
        {
            Notification n = context.Notifications.FirstOrDefault(n => n.NotificationId == id);

            if (n == null)
            {
                return NotFound("Notification not found");
            }

            n.NotificationMessage = newNotification.NotificationMessage;
            n.Type = newNotification.Type;

            context.SaveChanges();

            return Ok();
        }
        
        // Case 3 (PUT/PATCH): Mark notification status as "Read" or "Unread".
        [HttpPatch("UpdateNotificationStatus")]
        public IActionResult UpdateNotificationStatus(int id, string status)
        {
            Notification n = context.Notifications.FirstOrDefault(n => n.NotificationId == id);

            if (n == null)
            {
                return NotFound("Notification not found");
            }
            
            if (status != "Read" && status != "Unread")
            {
                return BadRequest("Status must be 'Read' or 'Unread'.");
            }

            n.Status = status;

            context.SaveChanges();

            return Ok();
        }
        // Case 4 (DELETE): Clear/delete a notification record.
        [HttpDelete("DeleteNotification")]
        public IActionResult DeleteNotification(int id)
        {
            Notification n = context.Notifications.FirstOrDefault(n => n.NotificationId == id);

            if (n == null)
            {
                return NotFound("Notification not found");
            }

            context.Notifications.Remove(n);

            context.SaveChanges();

            return Ok("Notification deleted successfully");
        }
        
        // Case 5 (GET - List): Fetch notifications including recipient User details.
        [HttpGet("GetAllNotifications")]
        public IActionResult GetAllNotifications()
        {
            List<Notification> notifications = context.Notifications
                .Include(n => n.User)
                .ToList();

            return Ok(notifications);
        }
        
        // Case 6 (GET - Find): Get notification details by NotificationId.
        [HttpGet("GetNotificationById")]
        public IActionResult GetNotificationById(int id)
        {
            Notification notification = context.Notifications
                .Include(n => n.User)
                .FirstOrDefault(n => n.NotificationId == id);

            if (notification == null)
            {
                return NotFound("Notification not found");
            }

            return Ok(notification);
        }
        
        // Case 7 (GET - Filter): Filter notifications to get unread items for a specific user.
        [HttpGet("GetUnreadNotifications")]
        public IActionResult GetUnreadNotifications(int userId)
        {
            List<Notification> notifications = context.Notifications
                .Where(n => n.UserId == userId && n.Status == "Unread")
                .Include(n => n.User)
                .ToList();

            return Ok(notifications);
        }
        
        // Case 8 (GET - Sort/Aggregate): Count total unread notifications for a user (Count).
    }
}
