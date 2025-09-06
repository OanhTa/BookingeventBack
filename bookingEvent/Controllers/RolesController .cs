using bookingEvent.Model;
using bookingEvent.Services;
using Microsoft.AspNetCore.Mvc;

namespace bookingEvent.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly RoleService _roleService;

        public RolesController(RoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _roleService.GetAllRolesAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            if (role == null) return NotFound();
            return Ok(role);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Role role)
        {
            var created = await _roleService.CreateRoleAsync(role);
            return Ok(created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, Role role)
        {
            role.Id = id;
            var result = await _roleService.UpdateRoleAsync(role);
            if (!result) return NotFound();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _roleService.DeleteRoleAsync(id);
            if (!result) return NotFound();
            return Ok();
        }

        [HttpPost("{id}/permissions")]
        public async Task<IActionResult> AssignPermissions(Guid id, [FromBody] List<Guid> permissionIds)
        {
            var result = await _roleService.AssignPermissionsToRoleAsync(id, permissionIds);
            if (!result) return NotFound();
            return Ok(new { message = "Assigned successfully" });
        }
    }
}
