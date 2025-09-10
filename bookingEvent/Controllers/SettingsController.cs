using bookingEvent.Services;
using Microsoft.AspNetCore.Mvc;

namespace bookingEvent.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SettingsController : ControllerBase
    {
        private readonly AppSettingService _service;

        public SettingsController(AppSettingService service)
        {
            _service = service;
        }

        [HttpGet("{prefix}")]
        public async Task<IActionResult> GetByPrefix(string prefix)
        {
            var result = await _service.GetSettingsByPrefixAsync(prefix);
            return Ok(result);
        }
    }
}
