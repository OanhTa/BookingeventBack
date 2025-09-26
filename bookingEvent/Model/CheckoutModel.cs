using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bookingEvent.Model
{
    public class CheckoutModel
    {
        [Key]
        public int Id { get; set; }   // Primary Key của bảng thanh toán

        // Liên kết với Event
        public Guid EventId { get; set; }

        // Thông tin người mua
        public string Ho { get; set; } = string.Empty;
        public string Ten { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string QuocGia { get; set; } = string.Empty;
        public string Thanhpho { get; set; } = string.Empty;
        public string Zip { get; set; } = string.Empty;

        // Thông tin thanh toán
        public string SoThe { get; set; } = string.Empty;
        public DateTime NgayHetHan { get; set; }
        public string CVV { get; set; } = string.Empty;
        public string? MaGiamGia { get; set; }

        // Đơn hàng
        public int SoLuong { get; set; }
        public decimal TongTien { get; set; }

        // Trạng thái
        public string TrangThai { get; set; } = "pending";

        // Hệ thống
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // 🔹 UserId: FK tới bảng Users (kiểu uniqueidentifier trong SQL)
        


        [ForeignKey("EventId")]
        public Event? Event { get; set; }
    }
}
