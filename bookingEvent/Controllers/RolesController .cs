using bookingEvent.DTO;
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
            try
            {
                var roles = await _roleService.GetAllRolesAsync();
                return Ok(ApiResponse<IEnumerable<RoleDto>>.SuccessResponse(
                    roles,
                    "Lấy danh sách vai trò thành công"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<object>.ErrorResponse("Lỗi hệ thống",
                        new List<string> { ex.Message },
                        StatusCodes.Status500InternalServerError));
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        [Permission("Identity.Roles.Read")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var role = await _roleService.GetRoleByIdAsync(id);
                if (role == null)
                    return NotFound(ApiResponse<object>.ErrorResponse("Không tìm thấy vai trò"));

                return Ok(ApiResponse<Role>.SuccessResponse(role, "Lấy thông tin vai trò thành công"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<object>.ErrorResponse("Lỗi hệ thống",
                        new List<string> { ex.Message },
                        StatusCodes.Status500InternalServerError));
            }
        }

        [HttpGet("search-key")]
        [Authorize]
        [Permission("Identity.Roles.Read")]
        public async Task<IActionResult> Search([FromQuery] string keyword)
        {
            try
            {
                var result = await _roleService.SearchRolesAsync(keyword);
                return Ok(ApiResponse<IEnumerable<Role>>.SuccessResponse(result, "Tìm kiếm vai trò thành công"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<object>.ErrorResponse("Lỗi hệ thống",
                        new List<string> { ex.Message },
                        StatusCodes.Status500InternalServerError));
            }
        }

        [HttpPost]
        [Authorize]
        [Permission("Identity.Roles.Create")]
        public async Task<IActionResult> Create(Role role)
        {
            try
            {
                var created = await _roleService.CreateRoleAsync(role);
                return Ok(ApiResponse<Role>.SuccessResponse(created, "Tạo vai trò thành công", StatusCodes.Status201Created));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<object>.ErrorResponse("Không thể tạo vai trò",
                        new List<string> { ex.Message },
                        StatusCodes.Status500InternalServerError));
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        [Permission("Identity.Roles.Update")]
        public async Task<IActionResult> Update(Guid id, Role role)
        {
            try
            {
                role.Id = id;
                var result = await _roleService.UpdateRoleAsync(role);
                if (!result)
                    return NotFound(ApiResponse<object>.ErrorResponse("Không tìm thấy vai trò để cập nhật"));

                return Ok(ApiResponse<object>.SuccessResponse(null, "Cập nhật vai trò thành công"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<object>.ErrorResponse("Không thể cập nhật vai trò",
                        new List<string> { ex.Message },
                        StatusCodes.Status500InternalServerError));
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        [Permission("Identity.Roles.Delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var result = await _roleService.DeleteRoleAsync(id);
                if (!result)
                    return NotFound(ApiResponse<object>.ErrorResponse("Không tìm thấy vai trò để xóa"));

                return Ok(ApiResponse<object>.SuccessResponse(null, "Xóa vai trò thành công"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<object>.ErrorResponse("Không thể xóa vai trò",
                        new List<string> { ex.Message },
                        StatusCodes.Status500InternalServerError));
            }
        }

        [HttpPost("{id}/permissions")]
        [Authorize]
        [Permission("Identity.Roles.Manage")]
        public async Task<IActionResult> AssignPermissions(Guid id, [FromBody] List<Guid> permissionIds)
        {
            try
            {
                var result = await _roleService.AssignPermissionsToRoleAsync(id, permissionIds);
                if (!result)
                    return NotFound(ApiResponse<object>.ErrorResponse("Không tìm thấy vai trò để gán quyền"));

                return Ok(ApiResponse<object>.SuccessResponse(null, "Gán quyền thành công"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<object>.ErrorResponse("Không thể gán quyền",
                        new List<string> { ex.Message },
                        StatusCodes.Status500InternalServerError));
            }
        }

        [HttpPost("move-users")]
        [Authorize]
        [Permission("Identity.Roles.Manage")]
        public async Task<IActionResult> MoveUsers([FromBody] MoveUsersRequest request)
        {
            try
            {
                var count = await _roleService.MoveUsersToRoleAsync(request.OldRoleId, request.NewRoleId);
                if (count == 0)
                    return NotFound(ApiResponse<object>.ErrorResponse("Không tìm thấy user nào với role cũ"));

                return Ok(ApiResponse<object>.SuccessResponse(
                    new { count, request.OldRoleId, request.NewRoleId },
                    $"Đã di chuyển {count} user từ role {request.OldRoleId} sang {request.NewRoleId}"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponse<object>.ErrorResponse("Không thể di chuyển user",
                        new List<string> { ex.Message },
                        StatusCodes.Status500InternalServerError));
            }
        }
    }
}

public class MoveUsersRequest
{
    public Guid OldRoleId { get; set; }
    public Guid NewRoleId { get; set; }
}
