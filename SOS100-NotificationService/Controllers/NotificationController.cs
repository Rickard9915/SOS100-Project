using Microsoft.AspNetCore.Mvc;
using SOS100_NotificationService.Data;
using SOS100_NotificationService.Dtos;
using SOS100_NotificationService.Models;

namespace SOS100_NotificationService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly NotificationDbContext _context;

        public NotificationController(NotificationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{userId}")]
        public IActionResult GetNotificationsByUserId(int userId)
        {
            var notifications = _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToList();

            return Ok(notifications);
        }

        [HttpPost]
        public IActionResult CreateNotification(CreateNotificationDto dto)
        {
            var notification = new Notification
            {
                UserId = dto.UserId,
                ApplicationId = dto.ApplicationId,
                Status = dto.Status,
                Message = dto.Message,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.Notifications.Add(notification);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetNotificationsByUserId), new { userId = notification.UserId }, notification);
        }
        [HttpPut("{id}/read")]
        public IActionResult MarkAsRead(int id)
        {
            var notification = _context.Notifications.FirstOrDefault(n => n.Id == id);

            if (notification == null)
            {
                return NotFound("Notification hittades inte.");
            }

            notification.IsRead = true;
            _context.SaveChanges();

            return NoContent();
        }
    }
}