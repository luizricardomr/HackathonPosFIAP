using VideoQRCode.Core.Message;

namespace VideoQRCode.DAO.Services
{
    public interface IVideoService
    {
        Task ProcessaVideo(VideoMessage message);
    }
}
