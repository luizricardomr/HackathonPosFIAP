using VideoQRCode.Core;

namespace VideoQRCode.DAO.Services
{
    public interface IVideoService
    {
        Task ProcessaVideo(VideoMessage message);
    }
}
