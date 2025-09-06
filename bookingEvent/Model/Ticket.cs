namespace bookingEvent.Model
{
    public class Ticket
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public Guid TicketTypeId { get; set; }
        public Guid? UserId { get; set; }
        public string Status { get; set; } = "Available";// Available, Sold, Cancelled
        public string? Code { get; set; }
        public DateTime? SoldAt { get; set; }
        public Event Event { get; set; } = null!;
        public TicketType TicketType { get; set; } = null!;
        public User? User { get; set; } = null!;
    }
}
