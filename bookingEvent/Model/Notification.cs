using System.ComponentModel.DataAnnotations;

namespace bookingEvent.Model
{
    public class Notification : BaseEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid OrganisationId { get; set; }
        public string Title { get; set; } = null!;
        public string Message { get; set; } = null!;
        public NotificationType Type { get; set; }
        public Organisation Organisation { get; set; } = null!;
        public ICollection<NotificationReader> Readers { get; set; } = new List<NotificationReader>();
    }
}

public enum NotificationType
{
    General = 0,  // Thông báo chung
    Event = 1,    // Thông báo liên quan đến sự kiện
    System = 2    // Thông báo hệ thống
}
