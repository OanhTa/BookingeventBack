using bookingEvent.DTO;
using bookingEvent.Repositories;
using bookingEvent.Services;
using Microsoft.AspNetCore.Mvc;

namespace bookingEvent.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly ReportService _reportService;

        public ReportController(ReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("report-stats")]
        public async Task<IActionResult> GetUserStats()
        {
            try
            {
                var stats = await _reportService.GetUserStatsAsync();
                return Ok(ApiResponse<ReportDto>.SuccessResponse(
                    stats,
                    "Lấy thống kê người dùng thành công"
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
