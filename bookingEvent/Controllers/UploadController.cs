using bookingEvent.Services;
using Microsoft.AspNetCore.Mvc;

namespace bookingEvent.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : ControllerBase
    {
        private readonly CloudinaryService _cloudinaryService;

        public UploadController(CloudinaryService cloudinaryService)
        {
            _cloudinaryService = cloudinaryService;
        }

        [HttpPost("upload-avatar")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadAvatar([FromForm] UploadAvatarRequest up)
        {
            try
            {
                if (up.File == null || up.File.Length == 0)
                    return BadRequest(new { success = false, message = "File không hợp lệ." });

                var avatarUrl = await _cloudinaryService.UploadImageAsync(up.File, up.UserId);

                if (string.IsNullOrEmpty(avatarUrl))
                {
                    return BadRequest(new { success = false, message = "Không thể tải ảnh." });
                }

                return Ok(new { success = true, avatarUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    success = false,
                    message = "Có lỗi xảy ra trong quá trình tải ảnh.",
                    error = ex.Message
                });
            }
        }

        [HttpPost("upload-poster")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadEventPoster(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest(new { success = false, message = "File không hợp lệ." });

                var avatarUrl = await _cloudinaryService.UploadPosterTempAsync(file);

                if (string.IsNullOrEmpty(avatarUrl))
                {
                    return BadRequest(new { success = false, message = "Không thể tải ảnh." });
                }
                return Ok(new { success = true, avatarUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    success = false,
                    message = "Có lỗi xảy ra trong quá trình tải ảnh.",
                    error = ex.Message
                });
            }
        }

    }

}

public class UploadAvatarRequest
{
    public IFormFile File { get; set; }        // File upload
    public Guid UserId { get; set; }           // Truyền dưới dạng form field
}

