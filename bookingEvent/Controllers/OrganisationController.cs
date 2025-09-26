using bookingEvent.DTO;
using bookingEvent.Repositories;
using bookingEvent.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace bookingEvent.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class OrganisationController : ControllerBase
    {
        private readonly IOrganisationRepository _orgService;

        public OrganisationController(IOrganisationRepository orgService)
        {
            _orgService = orgService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrganisationDto dto)
        {
            try
            {
                var userId = User.GetUserId();
                var org = await _orgService.CreateOrganisationAsync(dto, userId);
                return Ok(ApiResponse<object>.SuccessResponse(org, "Tạo tổ chức thành công", StatusCodes.Status201Created));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Không thể tạo tổ chức", new List<string> { ex.Message }, 500));
            }
        }

        [HttpPost("invite-user")]
        public async Task<IActionResult> InviteUser([FromBody] InviteUserDto dto)
        {
            try
            {
                var result = await _orgService.InviteUserAsync(dto);
                return Ok(ApiResponse<object>.SuccessResponse(result, "Mời người dùng thành công"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Không thể mời người dùng", new List<string> { ex.Message }, 500));
            }
        }

        [HttpGet("my-organisation")]
        public async Task<IActionResult> GetMy()
        {
            try
            {
                var userId = User.GetUserId();
                var orgs = await _orgService.GetUserOrganisationsAsync(userId);
                return Ok(ApiResponse<object>.SuccessResponse(orgs, "Lấy danh sách tổ chức thành công"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Không thể lấy danh sách tổ chức", new List<string> { ex.Message }, 500));
            }
        }

        [HttpGet("users-by-organisation")]
        public async Task<IActionResult> GetUsersByOrganisation(Guid orgId)
        {
            try
            {
                var users = await _orgService.GetUsersByOrganisationAsync(orgId);
                return Ok(ApiResponse<object>.SuccessResponse(users, "Lấy danh sách user trong tổ chức thành công"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Không thể lấy danh sách user", new List<string> { ex.Message }, 500));
            }
        }

        [HttpGet("by-user/{userId}")]
        public async Task<IActionResult> GetOrganisationsByUser(Guid userId)
        {
            try
            {
                var orgs = await _orgService.GetOrganisationsByUserAsync(userId);
                return Ok(ApiResponse<object>.SuccessResponse(orgs, "Lấy tổ chức theo user thành công"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Không thể lấy tổ chức theo user", new List<string> { ex.Message }, 500));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CreateOrganisationDto dto)
        {
            try
            {
                var userId = User.GetUserId();
                var org = await _orgService.UpdateOrganisationAsync(id, dto, userId);
                if (org == null)
                    return NotFound(ApiResponse<object>.ErrorResponse("Organisation not found or not owner",
                        new List<string>(), 404));

                return Ok(ApiResponse<object>.SuccessResponse(org, "Cập nhật tổ chức thành công"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Không thể cập nhật tổ chức", new List<string> { ex.Message }, 500));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var userId = User.GetUserId();
                var success = await _orgService.DeleteOrganisationAsync(id, userId);
                if (!success)
                    return NotFound(ApiResponse<object>.ErrorResponse("Organisation not found or not owner",
                        new List<string>(), 404));

                return Ok(ApiResponse<object>.SuccessResponse(null, "Xóa tổ chức thành công"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Không thể xóa tổ chức", new List<string> { ex.Message }, 500));
            }
        }
    }
}
