namespace bookingEvent.DTO
{
    public class CheckoutDto
    {
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
        public string NgayHetHan { get; set; } = string.Empty; // Angular gửi dạng "MM/YY"
        public string CVV { get; set; } = string.Empty;
        public string? MaGiamGia { get; set; }

        // Đơn hàng
        public int SoLuong { get; set; }
        public decimal TongTien { get; set; }

        // 🔹 UserId: kiểu Guid (uniqueidentifier trong SQL)
        

    }
}
