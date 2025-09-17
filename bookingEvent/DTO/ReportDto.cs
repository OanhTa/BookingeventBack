using Swashbuckle.AspNetCore.SwaggerGen;

namespace bookingEvent.DTO
{
    public class ReportDto
    {
        public int TotalUsers { get; set; }
        public int NewUsersThisMonth { get; set; }
        public int ActiveUsers { get; set; }
        public int LockedUsers { get; set; }
        public Dictionary<string, int> ByRole { get; set; } = new();
        public List<DailyRegistrationDto> LogsByDate { get; set; }
        public List<DailyRegistrationDto> RegistrationsByDate { get; set; } = new();
    }
    public class DailyRegistrationDto
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }
    }

}
