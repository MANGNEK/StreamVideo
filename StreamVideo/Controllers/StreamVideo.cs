using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using StreamVideo.models;
using System.IO;

namespace StreamVideo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StreamVideo : Controller
    {
        private readonly string[] allowedVideos = new[] { "video1", "video2", "video3", "video4", "video5", "video6", "video7", "video8", "video9", "video10", "video11", "video12", "video13", "video14", "video15", "video16", "video17", "video18", "video19", "video20", "video21", "video22", "video23", "video24", "video25", "video26", "video27", "video28" };
        private readonly string videoFolder = Path.Combine(Directory.GetCurrentDirectory(), "Video");
        private const string UploadKey = "080321";
        [EnableCors("AllowAll")]
        [HttpGet("stream")]
        public IActionResult StreamVideoFile([FromQuery] string name)
        {
            if (string.IsNullOrEmpty(name) || !allowedVideos.Contains(name))
                return BadRequest("Invalid or missing video name");

            var filePath = Path.Combine(videoFolder, $"{name}.mp4");

            if (!System.IO.File.Exists(filePath))
                return NotFound("Video not found");

            var stream = System.IO.File.OpenRead(filePath);
            return File(stream, "video/mp4", enableRangeProcessing: true);
        }

        [EnableCors("AllowAll")]
        [RequestSizeLimit(2_147_483_648)] // 2GB, có thể chỉnh lớn hơn nếu cần
        [HttpPost("upload")]
        public async Task<IActionResult> UploadVideo([FromForm] UploadVideoRequest request)
        {
            if (request.Key != UploadKey)
                return Unauthorized("Invalid key");

            if (request.File == null || request.File.Length == 0)
                return BadRequest("No file uploaded");

            if (string.IsNullOrEmpty(request.Name) || !allowedVideos.Contains(request.Name))
                return BadRequest("Invalid or missing video name");

            if (!Directory.Exists(videoFolder))
                Directory.CreateDirectory(videoFolder);

            var filePath = Path.Combine(videoFolder, $"{request.Name}.mp4");

            // Sử dụng stream để ghi file lớn
            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 81920, useAsync: true))
            {
                await request.File.CopyToAsync(stream);
            }

            return Ok(new { message = $"Video '{request.Name}' updated successfully!" });
        }
    }
}
