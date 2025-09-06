using bookingEvent.Model;
using bookingEvent.Services;
using Microsoft.AspNetCore.Mvc;

namespace bookingEvent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly TicketService _service;

        public TicketController(TicketService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TicketType>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TicketType>> Get(Guid id)
        {
            var ticket = await _service.GetByIdAsync(id);
            if (ticket == null) return NotFound();
            return Ok(ticket);
        }

        [HttpPost]
        public async Task<ActionResult<TicketType>> Create(TicketType model)
        {
            var ticket = await _service.CreateAsync(model);
            return CreatedAtAction(nameof(Get), new { id = ticket.Id }, ticket);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, TicketType model)
        {
            var result = await _service.UpdateAsync(id, model);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpGet("{id}/customers")]
        public async Task<ActionResult<IEnumerable<User>>> GetCustomers(Guid id)
        {
            var customers = await _service.GetCustomersAsync(id);
            return Ok(customers);
        }
    }

}
