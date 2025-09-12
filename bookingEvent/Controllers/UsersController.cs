using bookingEvent.DTO;
using bookingEvent.Infrastructure.Middlewares;
using bookingEvent.Model;
using bookingEvent.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bookingEvent.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize]
        [Permission("Identity.Users.Read")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        [Authorize]
        [Permission("Identity.Users.Read")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost("search")]
        [Authorize]
        [Permission("Identity.Users.Read")]
        public async Task<IActionResult> Search([FromBody] UserFilterDto filter)
        {
            var users = await _userService.SearchUsersAsync(filter);
            return Ok(users);
        }

        [HttpGet("search-key")]
        [Authorize]
        [Permission("Identity.Users.Read")]
        public async Task<IActionResult> SearchKey([FromQuery] string keyword)
        {
            var result = await _userService.SearchUsersAsync(keyword);
            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        [Permission("Identity.Users.Create")]
        public async Task<IActionResult> Create(CreateUserDto user)
        {
            var created = await _userService.CreateUserAsync(user);
            return Ok(created);
        }

        [HttpPut("{id}")]
        [Authorize]
        [Permission("Identity.Users.Update")]
        public async Task<IActionResult> Update(Guid id, UpdateUserDto user)
        {
            if (id != user.Id) return BadRequest();
            var result = await _userService.UpdateUserAsync(user);
            if (!result) return NotFound();
            return Ok(user);
        }

        [HttpPut("{id}/set-password")]
        [Authorize]
        [Permission("Identity.Users.Update")]
        public async Task<IActionResult> SetPassword(Guid id, [FromBody] SetPasswordDto dto)
        {
            if (id != dto.Id) return BadRequest();

            var result = await _userService.SetPasswordAsync(dto);
            if (!result) return NotFound();

            return Ok(new { message = "Password updated successfully" });
        }

        [HttpDelete("{id}")]
        [Authorize]
        [Permission("Identity.Users.Delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result) return NotFound();
            return Ok();
        }

        [HttpPost("{userId}/assign-role/{roleId}")]
        [Authorize]
        [Permission("Identity.Users.Update")]
        public async Task<IActionResult> AssignRole(Guid userId, Guid roleId)
        {
            await _userService.AssignRoleToUserAsync(userId, roleId);
            return Ok();
        }

        [HttpGet("{userId}/roles")]
        [Authorize]
        [Permission("Identity.Users.Read")]
        public async Task<IActionResult> GetUserRoles(Guid userId)
        {
            var roles = await _userService.GetUserRolesAsync(userId);
            return Ok(roles);
        }

        [HttpGet("{userId}/permissions")]
        [Authorize]
        [Permission("Identity.Users.Read")]
        public async Task<IActionResult> GetUserPermissions(Guid userId)
        {
            var permissions = await _userService.GetUserPermissionsAsync(userId);
            return Ok(permissions);
        }
        [HttpPost("lock/{userId}")]
        [Authorize]
        [Permission("Identity.Users.Update")]
        public async Task<IActionResult> LockUser(Guid userId, [FromBody] LockUserRequest request)
        {
            var success = await _userService.LockUserAsync(userId, request.LockEnd);
            if (!success) return NotFound("User not found or cannot lock");

            return Ok(new { message = $"User {userId} locked until {request.LockEnd}" });
        }

        [HttpPost("unlock/{userId}")]
        [Authorize]
        [Permission("Identity.Users.Update")]
        public async Task<IActionResult> UnlockUser(Guid userId)
        {
            var success = await _userService.UnlockUserAsync(userId);
            if (!success) return NotFound("User not found or cannot unlock");

            return Ok(new { message = $"User {userId} unlocked" });
        }

    }
}

public class LockUserRequest
{
    public DateTimeOffset LockEnd { get; set; }
}