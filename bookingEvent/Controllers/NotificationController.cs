using bookingEvent.Model;
using bookingEvent.Services;
using Microsoft.AspNetCore.Mvc;

namespace bookingEvent.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly NotificationService _notificationService;

        public NotificationController(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetMyNotifications([FromQuery] Guid userId)
        {
            var notifications = await _notificationService.GetUserNotificationsAsync(userId);
            return Ok(notifications);
        }

        [HttpPost("read/{notificationId}")]
        public async Task<IActionResult> MarkAsRead(Guid notificationId, [FromQuery] Guid userId)
        {
            await _notificationService.MarkAsReadAsync(notificationId, userId);
            return Ok("Cập nhật thành công");
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationDto dto)
        {
            var notification = await _notificationService.CreateNotificationAsync(
                dto.OrganisationId,
                dto.Title,
                dto.Message,
                dto.Type
            );

            return Ok(notification);
        }
    }

}
public class CreateNotificationDto
{
    public Guid OrganisationId { get; set; }
    public string Title { get; set; } = null!;
    public string Message { get; set; } = null!;
    public NotificationType Type { get; set; } = NotificationType.General;
}
