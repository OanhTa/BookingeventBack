using System.ComponentModel.DataAnnotations;

namespace bookingEvent.Model
{
    public class Account
    {
        [Key]
        public Guid Id { get; set; }
        public String Name { get; set; }
        public String PassHash { get; set; }
        public String Email { get; set; }
        public String Phone { get; set; }
        public String? Address { get; set; }
        public Guid AccountGroupId { get; set; }
        public AccountGroup AccountGroup { get; set; } 
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
