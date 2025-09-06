namespace bookingEvent.DTO
{
    public class EventWithDetailDto
    {
        // Event fields
        public string Name { get; set; } = string.Empty;
        public decimal PriceFrom { get; set; }
        public DateTime? Date { get; set; }
        public TimeSpan? Time { get; set; }
        public TimeSpan? Duration { get; set; }
        public string? Thumbnail { get; set; }
        public string? Status { get; set; }
        public Guid? CategoryId { get; set; }

        public string Description { get; set; } = string.Empty;
        public string? Location { get; set; }
        public string? SpeakerOrPerformer { get; set; }
        public int? TicketQuantity { get; set; }
        public string? ContactInfo { get; set; }
        public string? Gallery { get; set; }
    }

}
