
using FFMpegCore;
using System.Drawing;
using VideoQRCode.Core;
using VideoQRCode.DAO.Infra.Repository;
using VideoQRCode.DAO.Utils;
using ZXing.Windows.Compatibility;

namespace VideoQRCode.DAO.Services
{
    public class VideoService : IVideoService
    {
        private readonly IVideoRepository _videoRepository;
        private readonly IConteudoVideoService _conteudoVideoService;
        private readonly IFrameExtractor _frameExtractor;
        private readonly IFrameProcessor _frameProcessor;

        public VideoService(IVideoRepository videoRepository, 
                            IConteudoVideoService conteudoVideoService,
                            IFrameExtractor frameExtractor,
                            IFrameProcessor frameProcessor)
        {
            _videoRepository = videoRepository;
            _conteudoVideoService = conteudoVideoService;
            _frameExtractor = frameExtractor;
            _frameProcessor = frameProcessor;
        }

        public async Task ProcessaVideo(VideoMessage message)
        {
            var frames = await _frameExtractor.ExtractFramesAsync(message.Path);
            var resultados = await _frameProcessor.ProcessFramesAsync(frames);

            await _conteudoVideoService.SalvarQrCodesAsync(message.Id, resultados);
            await _videoRepository.UpdateStatusAsync(message.Id, "Concluído");
        }
    }
}
