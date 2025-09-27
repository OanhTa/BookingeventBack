using bookingEvent.DTO;
using bookingEvent.Model;
using bookingEvent.Repositories;
using bookingEvent.Services;
using Microsoft.AspNetCore.Mvc;

namespace bookingEvent.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly IEventRepository _eventService;

        public EventsController(IEventRepository eventService)
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

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string? keyword = null)
        {
            try
            {
                var events = await _eventService.SearchAsync(keyword);
                return Ok(events);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi khi tìm kiếm sự kiện.",
                    details = ex.Message
                });
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
        public async Task<IActionResult> GetByOrganisation(Guid orgId, [FromQuery] EventStatus? status)
        {
            try
            {
                var events = await _eventService.GetEventsByOrganisationAsync(orgId, status);
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
                    CategoryId = dto.CategoryId,
                    OrganisationId = dto.OrganisationId
                };

                var detail = new EventDetail
                {
                    Description = dto.Description,
                    Location = dto.Location,
                    SpeakerOrPerformer = dto.SpeakerOrPerformer,
                    ContactInfo = dto.ContactInfo,
                    Gallery = dto.Gallery
                };
                var tickets = dto.TicketTypes;
                var created = await _eventService.CreateAsync(newEvent, detail, tickets);
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
