using System.ComponentModel.DataAnnotations;

namespace bookingEvent.Model
{
    public class AuditLog
    {
        [Key]
        public Guid Id { get; set; }
        public string Action { get; set; }          
        public string Entity { get; set; }         
        public string EntityId { get; set; }        
        public string PerformedBy { get; set; }    
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string Description { get; set; }
    }
}
