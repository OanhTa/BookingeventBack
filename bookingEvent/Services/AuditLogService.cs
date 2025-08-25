using AutoMapper;
using bookingEvent.Data;
using bookingEvent.Model;
using Microsoft.EntityFrameworkCore;

namespace bookingEvent.Services
{
    public class AuditLogService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public AuditLogService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task LogAsync(AuditLog auditLog)
        {
            auditLog.Id = Guid.NewGuid();
            auditLog.Timestamp = DateTime.UtcNow;

            _context.AuditLog.Add(auditLog);
            await _context.SaveChangesAsync();
        }

        public async Task<List<AuditLog>> GetAllAsync()
        {
            var logs = await _context.AuditLog
                .OrderByDescending(x => x.Timestamp)
                .ToListAsync();

            return logs;
        }
    }
}
