using Microsoft.AspNetCore.Mvc;
using VideoQRCode.API.Infra.Repository;

namespace VideoQRCode.API.Controllers
{
    public class ConteudoVideoController : ControllerBase
    {
        private readonly IConteudoVideoRepository _conteudoRepository;

        public ConteudoVideoController(IConteudoVideoRepository conteudoRepository)
        {
            _conteudoRepository = conteudoRepository;
        }

        [HttpGet("GetConteudo/{videoId}")]
        public async Task<IActionResult> GetConteudoById(Guid videoId)
        {
            var videos = await _conteudoRepository.GetByVideoIdAsync(videoId);
            return Ok(videos);
        }
    }
}
