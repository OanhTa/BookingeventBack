using bookingEvent.DTO;
using bookingEvent.Model;
using bookingEvent.Repositories;
using bookingEvent.Services;
using Microsoft.AspNetCore.Mvc;

namespace bookingEvent.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuditLogController : ControllerBase
    {
        private IAuditLogRepository _auditLogService;
        public AuditLogController(IAuditLogRepository auditLogService)
        {
            _auditLogService = auditLogService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var logs = await _auditLogService.GetAllAsync();
            return Ok(logs);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AuditLog auditLog)
        {
            if (auditLog == null)
                return BadRequest("AuditLog is null");

            await _auditLogService.LogAsync(auditLog);
            return Ok(auditLog);
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody] AuditLogSearchDto search)
        {
            if (search == null)
                return BadRequest("Search object cannot be null.");

            var result = await _auditLogService.SearchAuditLogs(search);
            return Ok(result);
        }
    }
}
