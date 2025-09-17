using bookingEvent.Data;
using bookingEvent.DTO;
using Microsoft.EntityFrameworkCore;

namespace bookingEvent.Services
{
    public class ReportService
    {
        private readonly ApplicationDbContext _context;

        public ReportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ReportDto> GetUserStatsAsync()
        {
            var totalUsers = await _context.Users.CountAsync();

            // Active user = những user chưa bị khóa
            var activeUsers = await _context.Users
                .CountAsync(u => u.LockoutEnd == null || u.LockoutEnd <= DateTimeOffset.UtcNow);

            // Locked user = bị khóa (LockoutEnd còn hạn)
            var lockedUsers = await _context.Users
                .CountAsync(u => u.LockoutEnd != null && u.LockoutEnd > DateTimeOffset.UtcNow);

            var startOfMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
            var newUsersThisMonth = await _context.Users
                .CountAsync(u => u.CreatedAt >= startOfMonth);

            // Nhóm theo Role (UserRoles join Role)
            var byRole = await _context.UserRoles
                .Include(ur => ur.Role)
                .GroupBy(ur => ur.Role.Name)
                .Select(g => new { Role = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Role, x => x.Count);

            // Đếm đăng ký theo ngày trong tháng
            var registrationsByDate = await _context.Users
                .Where(u => u.CreatedAt >= startOfMonth)
                .GroupBy(u => u.CreatedAt.Date)
                .Select(g => new DailyRegistrationDto
                {
                    Date = g.Key,
                    Count = g.Count()
                })
                .OrderBy(x => x.Date)
                .ToListAsync();

            var logsByDate = await _context.AuditLog
               .GroupBy(l => l.ExecutionTime.Date) // nhóm theo ngày
               .Select(g => new DailyRegistrationDto
               {
                   Date = g.Key,
                   Count = g.Count()
               })
               .OrderBy(x => x.Date)
               .ToListAsync();

            return new ReportDto
            {
                TotalUsers = totalUsers,
                NewUsersThisMonth = newUsersThisMonth,
                ActiveUsers = activeUsers,
                LockedUsers = lockedUsers,
                ByRole = byRole,
                LogsByDate = logsByDate,
                RegistrationsByDate = registrationsByDate
            };
        }


    }
}
