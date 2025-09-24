using bookingEvent.Services;
using bookingEvent.DTO;
using bookingEvent.Services.Auth;

using Microsoft.AspNetCore.Mvc;

namespace bookingEvent.Controllers
{

        [ApiController]
        [Route("api/[controller]")]
        public class OrganisationController : ControllerBase
        {
            private readonly OrganisationService _orgService;

        public OrganisationController(OrganisationService orgService)
            {
                _orgService = orgService;
            }

            [HttpPost]
            public async Task<IActionResult> Create([FromBody] CreateOrganisationDto dto)
            {
                var userId = User.GetUserId();
                var org = await _orgService.CreateOrganisationAsync(dto, userId);
                return Ok(org);
            }

            [HttpGet("my-organisation")]
            public async Task<IActionResult> GetMy()
            {
                var userId = User.GetUserId();
                var orgs = await _orgService.GetUserOrganisationsAsync(userId);
                return Ok(orgs);
            }

            [HttpGet("users-by-organisation")]
            public async Task<IActionResult> GetUsersByOrganisation(Guid orgId)
            {
                var users = await _orgService.GetUsersByOrganisationAsync(orgId);
                return Ok(users);
            }

            [HttpGet("by-user/{userId}")]
                public async Task<IActionResult> GetOrganisationsByUser(Guid userId)
                {
                    var orgs = await _orgService.GetOrganisationsByUserAsync(userId);
    
                    return Ok(orgs);
                }


            [HttpPut("{id}")]
                public async Task<IActionResult> Update(Guid id, [FromBody] CreateOrganisationDto dto)
                {
                    var userId = User.GetUserId();
                    var org = await _orgService.UpdateOrganisationAsync(id, dto, userId);
                    if (org == null) return NotFound("Organisation not found or not owner");
                    return Ok(org);
                }

            [HttpDelete("{id}")]
            public async Task<IActionResult> Delete(Guid id)
            {
                var userId = User.GetUserId();
                var success = await _orgService.DeleteOrganisationAsync(id, userId);
                if (!success) return NotFound("Organisation not found or not owner");
                return NoContent();
            }
            
        }
    }
