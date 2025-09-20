namespace bookingEvent.Model
{
    public class TicketType
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int Sold { get; set; } = 0;

        public Event? Event { get; set; }
        public ICollection<Ticket>? Tickets { get; set; } = new List<Ticket>();
    }
}
