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

        [HttpPost("set")]
        public async Task<IActionResult> SetValue([FromBody] SetValueRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return BadRequest("Name is required");

            await _service.SetValueAsync(request.Name, request.Value, request.ProviderName, request.ProviderKey);
            return Ok(new { success = true });
        }

        [HttpPost("set-values")]
        public async Task<IActionResult> SetValues([FromBody] List<SetValueRequest> request)
        {
            foreach (var item in request)
            {
                await _service.SetValueAsync(item.Name, item.Value, item.ProviderName, item.ProviderKey);
            }

            return Ok(new { success = true });
        }


    }
}
public class SetValueRequest
{
    public string Name { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string? ProviderName { get; set; }
    public string? ProviderKey { get; set; }
}
