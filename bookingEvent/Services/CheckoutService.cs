using bookingEvent.Data;
using bookingEvent.DTO;
using bookingEvent.Model;
using Microsoft.EntityFrameworkCore;

namespace bookingEvent.Services
{
    public interface ICheckoutService
    {
        Task<CheckoutModel> CreateCheckoutAsync(CheckoutDto dto);
        Task<CheckoutModel?> GetCheckoutByIdAsync(int id);
    }

    public class CheckoutService : ICheckoutService
    {
        private readonly ApplicationDbContext _context;

        public CheckoutService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CheckoutModel> CreateCheckoutAsync(CheckoutDto dto)
        {
           

            var checkout = new CheckoutModel
            {
                EventId = dto.EventId,
                Ho = dto.Ho,
                Ten = dto.Ten,
                Email = dto.Email,
                Address = dto.Address,
                QuocGia = dto.QuocGia,
                Thanhpho = dto.Thanhpho,
                Zip = dto.Zip,
                SoThe = dto.SoThe,
                NgayHetHan = ParseExpiry(dto.NgayHetHan),
                CVV = dto.CVV,
                MaGiamGia = dto.MaGiamGia,
                SoLuong = dto.SoLuong,
                TongTien = dto.TongTien,
                TrangThai = "pending",
                CreatedAt = DateTime.Now
            };

            _context.thanhtoan.Add(checkout);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Outer Exception: " + ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Inner Exception: " + ex.InnerException.Message);
                }
                throw;
            }

            return checkout;
        }

        public async Task<CheckoutModel?> GetCheckoutByIdAsync(int id)
        {
            return await _context.thanhtoan
                .Include(c => c.Event)  // load thêm thông tin event
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        // Chuyển string "MM/YY" -> DateTime cuối tháng
        private DateTime ParseExpiry(string expiry)
        {
            if (string.IsNullOrWhiteSpace(expiry))
                return DateTime.Now;

            var parts = expiry.Split('/');
            if (parts.Length != 2)
                return DateTime.Now;

            if (!int.TryParse(parts[0], out int month)) month = DateTime.Now.Month;
            if (!int.TryParse(parts[1], out int year)) year = DateTime.Now.Year % 100;

            year += 2000; // "25" -> 2025

            return new DateTime(year, month, DateTime.DaysInMonth(year, month));
        }
    }
}
