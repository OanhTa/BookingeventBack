namespace bookingEvent.Model
{
    public class NotificationReader : BaseEntity
    {
        public Guid NotificationId { get; set; }
        public Guid UserId { get; set; }
        public bool IsRead { get; set; } = false;
        public Notification Notification { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
