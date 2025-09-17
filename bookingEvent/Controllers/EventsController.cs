using bookingEvent.DTO;
using bookingEvent.Model;
using bookingEvent.Services;
using Microsoft.AspNetCore.Mvc;

namespace bookingEvent.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly EventService _eventService;

        public EventsController(EventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var events = await _eventService.GetAllAsync();
                return Ok(events);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy danh sách sự kiện.", details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var ev = await _eventService.GetByIdAsync(id);
                if (ev == null) return NotFound(new { message = "Không tìm thấy sự kiện." });
                return Ok(ev);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy sự kiện.", details = ex.Message });
            }
        }

        [HttpGet("by-org/{orgId}")]
        public async Task<IActionResult> GetByOrganisation(Guid orgId)
        {
            try
            {
                var events = await _eventService.GetEventsByOrganisationAsync(orgId);
                return Ok(events);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy sự kiện theo tổ chức.", details = ex.Message });
            }
        }

        [HttpGet("by-user/{userId}")]
        public async Task<IActionResult> GetByUser(Guid userId)
        {
            try
            {
                var events = await _eventService.GetEventsByUserAsync(userId);
                return Ok(events);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy sự kiện theo người dùng.", details = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EventWithDetailDto dto)
        {
            try
            {
                var newEvent = new Event
                {
                    Name = dto.Name,
                    PriceFrom = dto.PriceFrom,
                    Date = dto.Date,
                    Time = dto.Time,
                    Duration = dto.Duration,
                    Thumbnail = dto.Thumbnail,
                    Status = dto.Status,
                    CategoryId = dto.CategoryId
                };

                var detail = new EventDetail
                {
                    Description = dto.Description,
                    Location = dto.Location,
                    SpeakerOrPerformer = dto.SpeakerOrPerformer,
                    ContactInfo = dto.ContactInfo,
                    Gallery = dto.Gallery
                };

                var created = await _eventService.CreateAsync(newEvent, detail);
                return Ok(created);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi tạo sự kiện.", details = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Event updatedEvent)
        {
            try
            {
                var result = await _eventService.UpdateAsync(id, updatedEvent);
                if (result == null) return NotFound(new { message = "Không tìm thấy sự kiện để cập nhật." });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi cập nhật sự kiện.", details = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var deleted = await _eventService.DeleteAsync(id);
                if (!deleted) return NotFound(new { message = "Không tìm thấy sự kiện để xóa." });
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi xóa sự kiện.", details = ex.Message });
            }
        }
    }
}
