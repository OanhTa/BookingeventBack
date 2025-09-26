using bookingEvent.Data;
using bookingEvent.Model;
using Microsoft.EntityFrameworkCore;
using System;

namespace bookingEvent.Services
{
    public class NotificationService
    {
        private readonly ApplicationDbContext _dbContext;

        public NotificationService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Tạo thông báo cho cả Organisation
        public async Task<Notification> CreateNotificationAsync(Guid organisationId, string title, string message, NotificationType type)
        {
            var notification = new Notification
            {
                OrganisationId = organisationId,
                Title = title,
                Message = message,
                Type = type
            };

            _dbContext.Notification.Add(notification);

            // Lấy danh sách thành viên Organisation
            var members = await _dbContext.OrganisationUser
                .Where(m => m.OrganisationId == organisationId)
                .ToListAsync();

            foreach (var member in members)
            {
                _dbContext.NotificationReader.Add(new NotificationReader
                {
                    Notification = notification,
                    UserId = member.UserId,
                    IsRead = false
                });
            }

            await _dbContext.SaveChangesAsync();
            return notification;
        }

        // Lấy danh sách thông báo của 1 user
        public async Task<List<NotificationReader>> GetUserNotificationsAsync(Guid userId)
        {
            return await _dbContext.NotificationReader
                .Include(r => r.Notification)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.Notification.CreatedAt)
                .ToListAsync();
        }

        // Đánh dấu đã đọc
        public async Task MarkAsReadAsync(Guid notificationId, Guid userId)
        {
            var reader = await _dbContext.NotificationReader
                .FirstOrDefaultAsync(r => r.NotificationId == notificationId && r.UserId == userId);

            if (reader != null)
            {
                reader.IsRead = true;
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
