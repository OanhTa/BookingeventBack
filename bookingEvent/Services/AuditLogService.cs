using AutoMapper;
using bookingEvent.Data;
using bookingEvent.DTO;
using bookingEvent.Model;
using bookingEvent.Repositories;
using Microsoft.EntityFrameworkCore;

namespace bookingEvent.Services
{
    public class AuditLogService : IAuditLogRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AuditLogService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Ghi log
        public async Task LogAsync(AuditLog auditLog)
        {
            auditLog.Id = Guid.NewGuid();
            auditLog.ExecutionTime = DateTime.UtcNow;

            _context.AuditLog.Add(auditLog);
            await _context.SaveChangesAsync();
        }

        // Lấy tất cả log
        public async Task<List<AuditLog>> GetAllAsync()
        {
            var logs = await _context.AuditLog
                .OrderByDescending(x => x.ExecutionTime)
                .ToListAsync();

            return logs;
        }

        public async Task<List<AuditLog>> SearchAuditLogs(AuditLogSearchDto search)
        {
            var query = _context.AuditLog.AsQueryable();

            if (!string.IsNullOrEmpty(search.UserName))
                query = query.Where(x => x.UserName!.Contains(search.UserName));

            if (!string.IsNullOrEmpty(search.ApplicationName))
                query = query.Where(x => x.ApplicationName!.Contains(search.ApplicationName));

            if (!string.IsNullOrEmpty(search.HttpMethod))
                query = query.Where(x => x.HttpMethod == search.HttpMethod);

            if (!string.IsNullOrEmpty(search.Url))
                query = query.Where(x => x.Url!.Contains(search.Url));

            if (search.StatusCode.HasValue)
                query = query.Where(x => x.StatusCode == search.StatusCode.Value);

            if (!string.IsNullOrEmpty(search.ClientIpAddress))
                query = query.Where(x => x.ClientIpAddress!.Contains(search.ClientIpAddress));

            if (!string.IsNullOrEmpty(search.CorrelationId))
                query = query.Where(x => x.CorrelationId!.Contains(search.CorrelationId));

            if (search.StartDate.HasValue)
                query = query.Where(x => x.ExecutionTime >= search.StartDate.Value);

            if (search.EndDate.HasValue)
                query = query.Where(x => x.ExecutionTime <= search.EndDate.Value);

            if (search.MinDuration.HasValue)
                query = query.Where(x => x.ExecutionDuration >= search.MinDuration.Value);

            if (search.MaxDuration.HasValue)
                query = query.Where(x => x.ExecutionDuration <= search.MaxDuration.Value);

            if (search.HasException.HasValue)
                query = query.Where(x => x.HasException == search.HasException.Value);

            return await query.ToListAsync();
        }

    }
}
