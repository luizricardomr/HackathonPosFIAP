using VideoQRCode.Core;

namespace VideoQRCode.API.Producers
{
    public interface IVideoProducer
    {
        Task ExecuteAsync(VideoMessage message);
    }
}
