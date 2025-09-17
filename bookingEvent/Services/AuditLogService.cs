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

        public async Task LogAsync(AuditLog auditLog)
        {
            auditLog.Id = Guid.NewGuid();
            auditLog.ExecutionTime = DateTime.UtcNow;

            _context.AuditLog.Add(auditLog);
            await _context.SaveChangesAsync();
        }

        public async Task<(List<AuditLog> Logs, int TotalCount)> GetPagedAsync(int page, int pageSize)
        {
            var query = _context.AuditLog.OrderByDescending(x => x.ExecutionTime);
            var totalCount = await query.CountAsync();
            var logs = await query
                .Skip((page - 1) * pageSize)//bỏ bản ghi
                .Take(pageSize)//lấy bản ghi
                .ToListAsync();

            return (logs, totalCount);
        }

        public async Task<(List<AuditLog> Logs, int TotalCount)> SearchAuditLogs(AuditLogSearchDto search, int page, int pageSize)
        {
            var query = _context.AuditLog.AsQueryable();

            if (!string.IsNullOrEmpty(search.keyWord))
                query = query.Where(x => x.UserName!.Contains(search.keyWord) 
                || x.ApplicationName!.Contains(search.keyWord) 
                || x.Url!.Contains(search.keyWord)
                || x.ClientIpAddress!.Contains(search.keyWord)
                || x.CorrelationId!.Contains(search.keyWord));

            if (!string.IsNullOrEmpty(search.HttpMethod))
                query = query.Where(x => x.HttpMethod == search.HttpMethod);

            if (search.StatusCode.HasValue)
                query = query.Where(x => x.StatusCode == search.StatusCode.Value);

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

            var totalCount = await query.CountAsync();

            var logs = await query
                .OrderByDescending(x => x.ExecutionTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (logs, totalCount);
        }

    }
}
