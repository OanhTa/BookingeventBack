using System.ComponentModel.DataAnnotations;

namespace bookingEvent.Model
{
    public class NguoiDung
    {
        [Key]
        public Guid ma { get; set; }
        public String ten { get; set; }
        public String matKhauHash { get; set; }
        public String email { get; set; }
        public String sdt { get; set; }
        public String? diaChi { get; set; }
        public String vaiTro { get; set; } = "User"; // Admin, Origintal, Admin, etc.

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
