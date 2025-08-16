using MassTransit;
using VideoQRCode.Core;
using VideoQRCode.DAO.Services;

namespace VideoQRCode.DAO.Consumers
{
    public class VideoConsumer : IConsumer<VideoMessage>
    {
        private readonly IVideoService _service;

        public VideoConsumer(IVideoService service)
        {
            _service = service;
        }

        public async Task Consume(ConsumeContext<VideoMessage> context)
        {
            await _service.ProcessaVideo(context.Message);
        }
    }
}
