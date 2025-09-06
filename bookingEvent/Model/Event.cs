using System.ComponentModel.DataAnnotations;

namespace bookingEvent.Model
{
    public class Event
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal PriceFrom { get; set; }
        public DateTime? Date { get; set; }
        public TimeSpan? Time { get; set; }
        public TimeSpan? Duration { get; set; }
        public string? Thumbnail { get; set; }
        public string? Status { get; set; } //Draft,Active,Cancelled

        public Guid? CategoryId { get; set; }
        public Category? Category { get; set; }
        public EventDetail? EventDetail { get; set; }
        public ICollection<TicketType> TicketTypes { get; set; } = new List<TicketType>();

        public Guid OrganisationId { get; set; }  
        public Organisation Organisation { get; set; } = null!;
    }
}
