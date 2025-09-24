namespace bookingEvent.Model
{
    public abstract class BaseEntity
    {
        public string? CreatedBy { get; set; }   
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
