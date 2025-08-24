using bookingEvent.Model;
using bookingEvent.Services;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace bookingEvent.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountGroupController : ControllerBase
    {
        private AccountGroupServices _accountgroupService;
        //private IMapper _mapper;
        public AccountGroupController(AccountGroupServices accountgroupService)
        {
            _accountgroupService = accountgroupService;
            //_mapper = mapper;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAccountGroup()
        {
            var account = await _accountgroupService.GetAccountGroupAsync();
            return Ok(account);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddAccountGroup([FromBody] AccountGroup models)
        {
            models.Id = Guid.NewGuid();
            var result = await _accountgroupService.AddAccountGroupAsync(models);

            if (result)
                return Ok("Thêm danh nhóm thành công");
            else
                return StatusCode(500, "Có lỗi khi thêm");
        }
    }
}
