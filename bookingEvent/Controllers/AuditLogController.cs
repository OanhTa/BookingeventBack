using bookingEvent.Model;
using bookingEvent.Services;
using Microsoft.AspNetCore.Mvc;

namespace bookingEvent.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuditLogController : ControllerBase
    {
        private AuditLogService _auditLogService;
        public AuditLogController(AuditLogService auditLogService)
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
    }
}
