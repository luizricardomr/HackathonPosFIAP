using Microsoft.AspNetCore.Mvc;
using VideoQRCode.API.Infra.Repository;
using VideoQRCode.API.Producers;
using VideoQRCode.Core.Domain;
using VideoQRCode.Core.Message;
using VideoQRCode.Core.Utils;


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
        private readonly IFileStorageService _fileStorage;
        public VideoController(IVideoProducer producer,
                     IConfiguration config,
                     IVideoRepository videoRepository,
                     IConteudoVideoRepository conteudoRepository,
                     IFileStorageService fileStorage)
        {
            _producer = producer;
            _config = config;
            _videoRepository = videoRepository;
            _conteudoRepository = conteudoRepository;
            _fileStorage = fileStorage;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadVideo(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Nenhum arquivo enviado");

            if (!file.ContentType.StartsWith("video/"))
                return BadRequest("Formato inválido. Envie um arquivo de vídeo.");

            var fullPath = await _fileStorage.SaveFileAsync(file, "videos");

            var message = new VideoMessage
            {
                Id = Guid.NewGuid(),
                FileName = file.FileName,
                Path = fullPath,
                UploadedAt = DateTime.UtcNow
            };

            await _producer.ExecuteAsync(message);
            await _videoRepository.AddAsync(new Video(message));

            return Ok(new { message = "Vídeo enviado", path = fullPath });
            
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