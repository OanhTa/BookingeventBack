namespace bookingEvent.DTO
{
    public class AuditLogSearchDto
    {
        public string? UserName { get; set; }
        public string? ApplicationName { get; set; }
        public string? HttpMethod { get; set; }
        public string? Url { get; set; }
        public int? StatusCode { get; set; }
        public string? ClientIpAddress { get; set; }
        public string? CorrelationId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? MinDuration { get; set; }
        public int? MaxDuration { get; set; }
        public bool? HasException { get; set; }
    }
}
