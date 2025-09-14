using bookingEvent.Model;
using bookingEvent.Repositories;
using Microsoft.AspNetCore.Mvc;
using static bookingEvent.Const.Permissions;

namespace bookingEvent.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PermissionsController : ControllerBase
    {
        private readonly IPermissionRepository _permissionService;

        public PermissionsController(IPermissionRepository permissionService)
        {
            _permissionService = permissionService;
        }

        [HttpGet("RolePermissions/{roleId}")]
        public async Task<IActionResult> GetRolePermissions(Guid roleId)
        {
            var permissions = await _permissionService.GetRolePermissionsAsync(roleId);
            return Ok(permissions);
        }

        [HttpGet("UserPermissions/{userId}")]
        public async Task<IActionResult> GetUserPermissions(Guid userId)
        {
            var permissions = await _permissionService.GetUserPermissionsAsync(userId);
            return Ok(permissions);
        }
        [HttpPost("grant-users")]
        public async Task<IActionResult> GrantUserPermissions(Guid userId, [FromBody] List<string> permissionNames)
        {
            if (permissionNames == null || !permissionNames.Any())
                return BadRequest("Permission list cannot be empty");

            await _permissionService.GrantUserPermissionsAsync(userId, permissionNames);
            return Ok(new { message = "Permissions granted successfully" });
        }

        // Revoke nhiều quyền
        [HttpPost("revoke-users")]
        public async Task<IActionResult> RevokeUserPermissions(Guid userId, [FromBody] List<string> permissionNames)
        {
            if (permissionNames == null || !permissionNames.Any())
                return BadRequest("Permission list cannot be empty");

            await _permissionService.RevokeUserPermissionsAsync(userId, permissionNames);
            return Ok(new { message = "Permissions revoked successfully" });
        }

        [HttpPost("grant-roles")]
        public async Task<IActionResult> GrantRolePermissionsAsync(Guid roleId, [FromBody] List<string> permissionNames)
        {
            if (permissionNames == null || !permissionNames.Any())
                return BadRequest("Permission list cannot be empty");

            await _permissionService.GrantRolePermissionsAsync(roleId, permissionNames);
            return Ok(new { message = "Permissions granted successfully" });
        }

        [HttpPost("revoke-roles")]
        public async Task<IActionResult> RevokeRolePermissions(Guid roleId, [FromBody] List<string> permissionNames)
        {
            if (permissionNames == null || !permissionNames.Any())
                return BadRequest("Permission list cannot be empty");

            await _permissionService.RevokeRolePermissionsAsync(roleId, permissionNames);
            return Ok(new { message = "Permissions revoked successfully" });
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _permissionService.GetAllPermissionsAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var permission = await _permissionService.GetPermissionByIdAsync(id);
            if (permission == null) return NotFound();
            return Ok(permission);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Permission permission)
        {
            var created = await _permissionService.CreatePermissionAsync(permission);
            return Ok(created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, Permission permission)
        {
            permission.Id = id;
            var result = await _permissionService.UpdatePermissionAsync(permission);
            if (!result) return NotFound();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _permissionService.DeletePermissionAsync(id);
            if (!result) return NotFound();
            return Ok();
        }
    }
}
 