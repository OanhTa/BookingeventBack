using bookingEvent.Model;
using bookingEvent.Services;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace bookingEvent.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountGroupPermissionsController : ControllerBase
    {
        private AccountGroupPermissionServices _permissionService;
        //private IMapper _mapper;
        public AccountGroupPermissionsController(AccountGroupPermissionServices permissionService)
        {
            _permissionService = permissionService;
            //_mapper = mapper;
        }

        [HttpGet("{groupId}")]
        public async Task<IActionResult> GetGroupPermissions(Guid groupId)
        {
            var permissions = await _permissionService.GetPermissionsByGroupAsync(groupId);
            return Ok(permissions);
        }

        [HttpPost("add-multi")]
        public async Task<IActionResult> AddPermissions([FromBody] List<AccountGroupPermissions> models)
        {
            if (models == null || !models.Any())
                return BadRequest("Danh sách quyền rỗng");

            var result = await _permissionService.AddPermissionsAsync(models);

            if (result)
                return Ok("Thêm danh sách quyền thành công");
            else
                return StatusCode(500, "Có lỗi khi thêm quyền");
        }
    }
}
