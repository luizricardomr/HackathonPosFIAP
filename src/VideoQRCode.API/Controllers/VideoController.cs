using Microsoft.AspNetCore.Mvc;
using VideoQRCode.API.Domain;
using VideoQRCode.API.Infra.Repository;
using VideoQRCode.API.Producers;
using VideoQRCode.Core;


namespace VideoQRCode.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VideoController : ControllerBase
    {
        private readonly IVideoProducer _producer;
        private readonly IConfiguration _config;
        private readonly IVideoRepository _videoRepository;
        private readonly IConteudoVideoRepository _conteudoRepository;
        public VideoController(IVideoProducer producer,
                     IConfiguration config,
                     IVideoRepository videoRepository,
                     IConteudoVideoRepository conteudoRepository)
        {
            _producer = producer;
            _config = config;
            _videoRepository = videoRepository;
            _conteudoRepository = conteudoRepository;
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
                Id = Guid.NewGuid(),
                FileName = file.FileName,
                Path = fullPath,
                UploadedAt = DateTime.UtcNow
            };

            await _producer.ExecuteAsync(message);
            await _videoRepository.AddAsync(new Video(message));

            return Ok(new { message = "V�deo enviado", path = fullPath });
            
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var videos = await _videoRepository.GetAllAsync();
            return Ok(videos);
        }
        [HttpGet("GetConteudo/{videoId}")]
        public async Task<IActionResult> GetConteudoById(Guid videoId)
        {
            var videos = await _conteudoRepository.GetByVideoIdAsync(videoId);
            return Ok(videos);
        }
    }
}