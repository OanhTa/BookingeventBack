namespace bookingEvent.DTO
{
    public class EventDTO
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public decimal? PriceFrom { get; set; }
        public DateTime? Date { get; set; }
        public TimeSpan? Time { get; set; }
        public TimeSpan? Duration { get; set; }
        public string? Thumbnail { get; set; }
        public string? Status { get; set; }
        public Guid? CategoryId { get; set; }
    }
}
