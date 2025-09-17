using bookingEvent.DTO;
using bookingEvent.Infrastructure.Middlewares;
using bookingEvent.Model;
using bookingEvent.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bookingEvent.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userService;
        private readonly IAuthRepository _authService;

        public UsersController(IUserRepository userService, IAuthRepository authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [HttpGet]
        [Authorize]
        [Permission("Identity.Users.Read")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return Ok(ApiResponse<IEnumerable<User>>.SuccessResponse(users, "Lấy danh sách người dùng thành công"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<object>.ErrorResponse("Lỗi hệ thống", new List<string> { ex.Message }, StatusCodes.Status500InternalServerError));
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        [Permission("Identity.Users.Read")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return Ok(ApiResponse<IEnumerable<User>>.SuccessResponse(users, "Lấy danh sách người dùng thành công"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<object>.ErrorResponse("Lỗi hệ thống", new List<string> { ex.Message }, StatusCodes.Status500InternalServerError));
            }
        }

        [HttpPost("search")]
        [Authorize]
        [Permission("Identity.Users.Read")]
        public async Task<IActionResult> Search([FromBody] UserFilterDto filter)
        {
            try
            {
                var users = await _userService.SearchUsersAsync(filter);
                return Ok(ApiResponse<IEnumerable<User>>.SuccessResponse(users, "Tìm kiếm người dùng thành công"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Lỗi hệ thống", new List<string> { ex.Message }, 500));
            }
        }

        [HttpGet("search-key")]
        [Authorize]
        [Permission("Identity.Users.Read")]
        public async Task<IActionResult> SearchKey([FromQuery] string keyword)
        {
            try
            {
                var result = await _userService.SearchUsersAsync(keyword);
                return Ok(ApiResponse<IEnumerable<User>>.SuccessResponse(result, "Tìm kiếm theo từ khóa thành công"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Lỗi hệ thống", new List<string> { ex.Message }, 500));
            }
        }

        [HttpPost]
        [Authorize]
        [Permission("Identity.Users.Create")]
        public async Task<IActionResult> Create(CreateUserDto user)
        {
            var validateResult = await _authService.Validate(user.PasswordHash);
            if (!validateResult.IsValid)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse("Mật khẩu không hợp lệ", validateResult.Errors,StatusCodes.Status400BadRequest));
            }
            try
            {
                var created = await _userService.CreateUserAsync(user);
                return Ok(ApiResponse<User>.SuccessResponse(created, "Tạo người dùng thành công", StatusCodes.Status201Created));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Không thể tạo người dùng", new List<string> { ex.Message }, 500));
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        [Permission("Identity.Users.Update")]
        public async Task<IActionResult> Update(Guid id, UpdateUserDto user)
        {
            if (id != user.Id)
                return BadRequest(ApiResponse<object>.ErrorResponse("Id không khớp", null, 400));
            try
            {
                var result = await _userService.UpdateUserAsync(user);
                if (!result)
                    return NotFound(ApiResponse<object>.ErrorResponse("Không tìm thấy người dùng để cập nhật", null, 404));

                return Ok(ApiResponse<UpdateUserDto>.SuccessResponse(user, "Cập nhật người dùng thành công"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Cập nhật thất bại", new List<string> { ex.Message }, 500));
            }
        }

        [HttpPut("profile/{id}")]
        [Authorize]
        [Permission("Identity.Users.Update")]
        public async Task<IActionResult> UpdateProfile(Guid id, UpdateUserDto user)
        {
            if (id != user.Id)
                return BadRequest(ApiResponse<object>.ErrorResponse("Id không khớp", null, 400));

            try
            {
                var result = await _userService.UpdateProfileAsync(user);
                if (!result)
                    return NotFound(ApiResponse<object>.ErrorResponse("Không tìm thấy user cần cập nhật", null, 404));

                return Ok(ApiResponse<UpdateUserDto>.SuccessResponse(user, "Cập nhật profile thành công"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Lỗi khi cập nhật profile", new List<string> { ex.Message }, 500));
            }
        }

        [HttpPut("{id}/set-password")]
        [Authorize]
        [Permission("Identity.Users.Update")]
        public async Task<IActionResult> SetPassword(Guid id, [FromBody] SetPasswordDto dto)
        {
            if (id != dto.Id)
                return BadRequest(ApiResponse<object>.ErrorResponse("Id không khớp", null, 400));
            try
            {
                var result = await _userService.SetPasswordAsync(dto);
                if (!result)
                    return NotFound(ApiResponse<object>.ErrorResponse("Không tìm thấy user để cập nhật mật khẩu", null, 404));

                return Ok(ApiResponse<object>.SuccessResponse(null, "Cập nhật mật khẩu thành công"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Lỗi khi cập nhật mật khẩu", new List<string> { ex.Message }, 500));
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        [Permission("Identity.Users.Delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var result = await _userService.DeleteUserAsync(id);
                if (!result)
                    return NotFound(ApiResponse<object>.ErrorResponse("Không tìm thấy người dùng để xóa", null, 404));

                return Ok(ApiResponse<object>.SuccessResponse(null, "Xóa người dùng thành công"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Lỗi khi xóa người dùng", new List<string> { ex.Message }, 500));
            }
        }

        [HttpPost("{userId}/assign-role/{roleId}")]
        [Authorize]
        [Permission("Identity.Users.Update")]
        public async Task<IActionResult> AssignRole(Guid userId, Guid roleId)
        {
            try
            {
                await _userService.AssignRoleToUserAsync(userId, roleId);
                return Ok(ApiResponse<object>.SuccessResponse(null, "Gán role thành công"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Lỗi khi gán role", new List<string> { ex.Message }, 500));
            }
        }

        [HttpGet("{userId}/roles")]
        [Authorize]
        [Permission("Identity.Users.Read")]
        public async Task<IActionResult> GetUserRoles(Guid userId)
        {
            try
            {
                var roles = await _userService.GetUserRolesAsync(userId);
                return Ok(ApiResponse<IEnumerable<Role>>.SuccessResponse(roles, "Lấy roles của user thành công"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Lỗi khi lấy roles", new List<string> { ex.Message }, 500));
            }
        }

        //[HttpGet("{userId}/permissions")]
        //[Authorize]
        //[Permission("Identity.Users.Read")]
        //public async Task<IActionResult> GetUserPermissions(Guid userId)
        //{
        //    try
        //    {
        //        var permissions = await _userService.GetUserPermissionsAsync(userId);
        //        return Ok(ApiResponse<IEnumerable<string>>.SuccessResponse(permissions, "Lấy quyền của user thành công"));
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ApiResponse<object>.ErrorResponse("Lỗi khi lấy quyền", new List<string> { ex.Message }, 500));
        //    }
        //}

        [HttpPost("lock/{userId}")]
        [Authorize]
        [Permission("Identity.Users.Update")]
        public async Task<IActionResult> LockUser(Guid userId, [FromBody] LockUserRequest request)
        {
            try
            {
                var success = await _userService.LockUserAsync(userId, request.LockEnd);
                if (!success)
                    return NotFound(ApiResponse<object>.ErrorResponse("User không tồn tại hoặc không thể khóa", null, 404));

                return Ok(ApiResponse<object>.SuccessResponse(null, $"Tài khoản sẽ bị khóa đến {request.LockEnd}"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Lỗi khi khóa user", new List<string> { ex.Message }, 500));
            }
        }

        [HttpPost("unlock/{userId}")]
        [Authorize]
        [Permission("Identity.Users.Update")]
        public async Task<IActionResult> UnlockUser(Guid userId)
        {
            try
            {
                var success = await _userService.UnlockUserAsync(userId);
                if (!success)
                    return NotFound(ApiResponse<object>.ErrorResponse("User không tồn tại hoặc không thể mở khóa", null, 404));

                return Ok(ApiResponse<object>.SuccessResponse(null, $"Tài khoản đã được mở khóa"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Lỗi khi mở khóa user", new List<string> { ex.Message }, 500));
            }
        }
    }
}

public class LockUserRequest
{
    public DateTimeOffset LockEnd { get; set; }
}