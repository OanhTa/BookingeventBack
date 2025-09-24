using System.ComponentModel.DataAnnotations;


namespace bookingEvent.Model
{
    public class EventDetail : BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public Event Event { get; set; }
        public string Description { get; set; }

        public string? Location { get; set; }
        public string? SpeakerOrPerformer { get; set; }
        public string? ContactInfo { get; set; }
        public string? Gallery { get; set; }
    }
}
