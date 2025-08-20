using MassTransit;
using VideoQRCode.Core.Message;
using VideoQRCode.DAO.Infra.Repository;
using VideoQRCode.DAO.Services;

namespace VideoQRCode.DAO.Consumers
{
    public class VideoConsumer : IConsumer<VideoMessage>
    {
        private readonly IVideoService _service;
        private readonly IVideoRepository _videoRepository;

        public VideoConsumer(IVideoService service,
                             IVideoRepository videoRepository)
        {
            _service = service;
            _videoRepository = videoRepository;
        }

        public async Task Consume(ConsumeContext<VideoMessage> context)
        {
            await _videoRepository.UpdateStatusAsync(context.Message.Id, "Processando");
            await _service.ProcessaVideo(context.Message);
        }
    }
}
