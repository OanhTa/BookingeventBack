using System.ComponentModel.DataAnnotations;

namespace bookingEvent.Model
{
    public class AuditLog
    {
        [Key]
        public Guid Id { get; set; }
        // Người thực hiện
        public string? UserName { get; set; }

        // Thông tin request
        public string? ApplicationName { get; set; }
        public string? HttpMethod { get; set; }
        public string? Url { get; set; }
        public int? StatusCode { get; set; }
        public string? ClientIpAddress { get; set; }
        public string? CorrelationId { get; set; }

        // Thời gian & hiệu năng
        public DateTime ExecutionTime { get; set; }
        public int ExecutionDuration { get; set; }

        // Kết quả
        public bool HasException { get; set; }
        public string? ExceptionMessage { get; set; }

        // Audit entity (CRUD)
        public string? Action { get; set; }
        public string? Entity { get; set; }
        public string? EntityId { get; set; }
        public string? Description { get; set; }
    }
}
