using bookingEvent.DTO;
using bookingEvent.Infrastructure.Middlewares;
using bookingEvent.Model;
using bookingEvent.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bookingEvent.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuditLogController : ControllerBase
    {
        private readonly IAuditLogRepository _auditLogService;

        public AuditLogController(IAuditLogRepository auditLogService)
        {
            _auditLogService = auditLogService;
        }

        [HttpGet]
        [Authorize]
        [Permission("Identity.AuditLog.Read")]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var (logs, totalCount) = await _auditLogService.GetPagedAsync(page, pageSize);
                return Ok(ApiResponse<object>.SuccessResponse(
                     new
                     {
                         Data = logs,
                         TotalCount = totalCount,
                         Page = page,
                         PageSize = pageSize
                     },
                     "Lấy danh sách audit log thành công"
                 ));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<object>.ErrorResponse(
                        "Lỗi hệ thống",
                        new List<string> { ex.Message },
                        StatusCodes.Status500InternalServerError
                    ));
            }
        }

        [HttpPost]
        [Authorize]
        [Permission("Identity.AuditLog.Create")]
        public async Task<IActionResult> Create([FromBody] AuditLog auditLog)
        {
            try
            {
                if (auditLog == null)
                    return BadRequest(ApiResponse<object>.ErrorResponse(
                        "AuditLog is null",
                        new List<string> { "Dữ liệu gửi lên không hợp lệ" },
                        StatusCodes.Status400BadRequest
                    ));

                await _auditLogService.LogAsync(auditLog);
                return Ok(ApiResponse<AuditLog>.SuccessResponse(
                    auditLog,
                    "Tạo audit log thành công"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<object>.ErrorResponse(
                        "Lỗi hệ thống",
                        new List<string> { ex.Message },
                        StatusCodes.Status500InternalServerError
                    ));
            }
        }

        [HttpPost("search")]
        [Authorize]
        [Permission("Identity.Users.Read")]
        public async Task<IActionResult> Search([FromBody] AuditLogSearchDto search, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                if (search == null)
                    return BadRequest(ApiResponse<object>.ErrorResponse(
                        "Search object cannot be null",
                        new List<string> { "Dữ liệu tìm kiếm không hợp lệ" },
                        StatusCodes.Status400BadRequest
                    ));

                var (logs, totalCount) = await _auditLogService.SearchAuditLogs(search, page, pageSize);
                return Ok(ApiResponse<object>.SuccessResponse(
                      new
                      {
                          Data = logs,
                          TotalCount = totalCount,
                          Page = page,
                          PageSize = pageSize
                      },
                      "Tìm kiếm audit log thành công"
                  ));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<object>.ErrorResponse(
                        "Lỗi hệ thống",
                        new List<string> { ex.Message },
                        StatusCodes.Status500InternalServerError
                    ));
            }
        }
    }
}
