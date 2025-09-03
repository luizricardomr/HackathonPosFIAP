using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VideoQRCode.Web.Models;
using VideoQRCode.Web.Services;

namespace VideoQRCode.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IVideoService _videoService;
        public HomeController(ILogger<HomeController> logger,
                              IVideoService videoService)
        {
            _logger = logger;
            _videoService = videoService;
        }

        public IActionResult Index()
        {
            ViewBag.Mensagem = null;
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile arquivo)
        {
            bool sucesso = await _videoService.EnviarVideoAsync(arquivo);

            ViewBag.Mensagem = sucesso
                ? $"Vídeo '{arquivo.FileName}' enviado com sucesso!"
                : "Falha ao enviar vídeo.";

            return View("Index");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
