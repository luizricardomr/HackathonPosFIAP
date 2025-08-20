using VideoQRCode.Core.Message;

namespace VideoQRCode.API.Producers
{
    public interface IVideoProducer
    {
        Task ExecuteAsync(VideoMessage message);
    }
}
