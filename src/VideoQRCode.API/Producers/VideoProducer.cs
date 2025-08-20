
using MassTransit;
using Microsoft.Extensions.Options;
using VideoQRCode.API.Configuration;
using VideoQRCode.Core.Message;

namespace VideoQRCode.API.Producers
{
    public class VideoProducer : IVideoProducer
    {
        private readonly IBus _bus;
        private readonly string _videoQueue;

        public VideoProducer(IBus bus, IOptions<MassTransitConfig> config)
        {
            _bus = bus;
            _videoQueue = config.Value.VideoQueue;
        }

        public async Task ExecuteAsync(VideoMessage message)
        {
            try
            {
                var uriQueue = "queue:" + _videoQueue;
                var endpoint = await _bus.GetSendEndpoint(new Uri(uriQueue));
                await endpoint.Send(message);
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }
    }
}
