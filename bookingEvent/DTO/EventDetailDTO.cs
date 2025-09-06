using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bookingEvent.DTO
{
    public class EventDetailDTO
    {
        public Guid? Id { get; set; }
        public Guid? EventId { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }
        public string? SpeakerOrPerformer { get; set; }
        public int? TicketQuantity { get; set; }
        public string? ContactInfo { get; set; }
        public string? Gallery { get; set; }
    }
}
