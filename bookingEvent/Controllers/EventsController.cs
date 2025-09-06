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
            var events = await _eventService.GetAllAsync();
            return Ok(events);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var ev = await _eventService.GetByIdAsync(id);
            if (ev == null) return NotFound();
            return Ok(ev);
        }

        [HttpGet("by-org/{orgId}")]
        public async Task<IActionResult> GetByOrganisation(Guid orgId)
        {
            var events = await _eventService.GetEventsByOrganisationAsync(orgId);
            return Ok(events);
        }

        [HttpGet("by-user/{userId}")]
        public async Task<IActionResult> GetByUser(Guid userId)
        {
            var events = await _eventService.GetEventsByUserAsync(userId);
            return Ok(events);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EventWithDetailDto dto)
        {
            var newEvent = new Event
            {
                Name = dto.Name,
                PriceFrom = dto.PriceFrom,
                Date = dto.Date,
                Time  = dto.Time,
                Duration   = dto.Duration,
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

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Event updatedEvent)
        {
            var result = await _eventService.UpdateAsync(id, updatedEvent);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _eventService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
