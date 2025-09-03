using MassTransit;
using Microsoft.AspNetCore.SignalR;
using VideoQRCode.Core.Message;
using VideoQRCode.DAO.Hubs;
using VideoQRCode.DAO.Infra.Repository;
using VideoQRCode.DAO.Services;

namespace VideoQRCode.DAO.Consumers
{
    public class VideoConsumer : IConsumer<VideoMessage>
    {
        private readonly IVideoService _service;
        private readonly IVideoRepository _videoRepository;
        private readonly INotificacaoService _notificacaoService;

        public VideoConsumer(IVideoService service,
                             IVideoRepository videoRepository,
                             INotificacaoService notificacaoService)
        {
            _service = service;
            _videoRepository = videoRepository;
            _notificacaoService = notificacaoService;
        }

        public async Task Consume(ConsumeContext<VideoMessage> context)
        {
            var fileName = context.Message.FileName;
            await _notificacaoService.NotificarStatusAsync(fileName, "Processando");
            await Task.Delay(3000);
            await _service.ProcessaVideo(context.Message);
            await Task.Delay(3000);
            await _notificacaoService.NotificarStatusAsync(fileName, "Concluído");
        }
    }
}
