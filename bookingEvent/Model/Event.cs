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
        public EventStatus Status { get; set; } = EventStatus.Draft;

        public Guid? CategoryId { get; set; }
        public Category? Category { get; set;  }
        public EventDetail? EventDetail { get; set; }
        public ICollection<TicketType> TicketTypes { get; set; } = new List<TicketType>();

        public Guid? OrganisationId { get; set; }  
        public Organisation? Organisation { get; set; } = null!;
    }
}
public enum EventStatus
{
    Draft = 0,        // Nháp - tạo xong nhưng chưa công khai
    Published = 1,    // Đã xuất bản - đang hiển thị cho khách
    Ongoing = 2,      // Đang diễn ra
    Completed = 3,    // Đã kết thúc
    Cancelled = 4,    // Đã hủy
    Archived = 5      // Đã lưu trữ
}
