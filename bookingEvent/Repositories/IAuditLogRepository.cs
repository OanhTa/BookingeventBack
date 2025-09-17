using bookingEvent.DTO;
using bookingEvent.Model;
using Microsoft.AspNetCore.Mvc;
namespace bookingEvent.Repositories
{
    public interface IAuditLogRepository
    {
        Task<(List<AuditLog> Logs, int TotalCount)> SearchAuditLogs(AuditLogSearchDto search, int page, int pageSize);
        Task LogAsync(AuditLog auditLog);
        Task<(List<AuditLog> Logs, int TotalCount)> GetPagedAsync(int page, int pageSize);
    }
}
