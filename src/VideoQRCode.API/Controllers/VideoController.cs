using Microsoft.AspNetCore.Mvc;
using VideoQRCode.API.Producers;
using VideoQRCode.Core;


namespace VideoQRCode.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Video : ControllerBase
    {
        private readonly IVideoProducer _producer;
        private readonly IConfiguration _config;
        public Video(IVideoProducer producer,
                     IConfiguration config)
        {
            _producer = producer;
            _config = config;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadVideo(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Nenhum arquivo enviado");

            var uploadsDir = _config["Storage:UploadPath"];
            if (!Directory.Exists(uploadsDir))
                Directory.CreateDirectory(uploadsDir);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var fullPath = Path.Combine(uploadsDir, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var message = new VideoMessage
            {
                FileName = file.FileName,
                Path = fullPath,
                UploadedAt = DateTime.UtcNow
            };

            await _producer.ExecuteAsync(message);

            return Ok(new { message = "Vídeo enviado", path = fullPath });
            
        }           
        
    }
}