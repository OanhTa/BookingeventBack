using bookingEvent.DTO;
using bookingEvent.Model;
using Microsoft.AspNetCore.Mvc;
namespace bookingEvent.Repositories
{
    public interface IAuditLogRepository
    {
        Task<List<AuditLog>> SearchAuditLogs(AuditLogSearchDto search);
        Task LogAsync(AuditLog auditLog);
        Task<List<AuditLog>> GetAllAsync();
    }
}
