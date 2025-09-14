using bookingEvent.Infrastructure.Middlewares;
using bookingEvent.Model;
using bookingEvent.Repositories;
using bookingEvent.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bookingEvent.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleRepository _roleService;

        public RolesController(IRoleRepository roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        [Authorize]
        [Permission("Identity.Roles.Read")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _roleService.GetAllRolesAsync());
        }

        [HttpGet("{id}")]
        [Authorize]
        [Permission("Identity.Roles.Read")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            if (role == null) return NotFound();
            return Ok(role);
        }

        [HttpGet("search-key")]
        [Authorize]
        [Permission("Identity.Roles.Read")]
        public async Task<IActionResult> Search([FromQuery] string keyword)
        {
            var result = await _roleService.SearchRolesAsync(keyword);
            return Ok(result);
        }


        [HttpPost]
        [Authorize]
        [Permission("Identity.Roles.Create")]
        public async Task<IActionResult> Create(Role role)
        {
            var created = await _roleService.CreateRoleAsync(role);
            return Ok(created);
        }

        [HttpPut("{id}")]
        [Authorize]
        [Permission("Identity.Roles.Update")]
        public async Task<IActionResult> Update(Guid id, Role role)
        {
            role.Id = id;
            var result = await _roleService.UpdateRoleAsync(role);
            if (!result) return NotFound();
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize]
        [Permission("Identity.Roles.Delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _roleService.DeleteRoleAsync(id);
            if (!result) return NotFound();
            return Ok();
        }

        [HttpPost("{id}/permissions")]
        [Authorize]
        [Permission("Identity.Roles.Manage")]
        public async Task<IActionResult> AssignPermissions(Guid id, [FromBody] List<Guid> permissionIds)
        {
            var result = await _roleService.AssignPermissionsToRoleAsync(id, permissionIds);
            if (!result) return NotFound();
            return Ok(new { message = "Assigned successfully" });
        }

        [HttpPost("move-users")]
        public async Task<IActionResult> MoveUsers([FromBody] MoveUsersRequest request)
        {
            var count = await _roleService.MoveUsersToRoleAsync(request.OldRoleId, request.NewRoleId);

            if (count == 0)
                return NotFound(new { message = "Không tìm thấy user nào với role cũ" });

            return Ok(new { message = $"Đã di chuyển {count} user từ role {request.OldRoleId} sang {request.NewRoleId}" });
        }
    }
}

public class MoveUsersRequest
{
    public Guid OldRoleId { get; set; }
    public Guid NewRoleId { get; set; }
}
