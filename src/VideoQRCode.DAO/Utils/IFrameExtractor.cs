namespace VideoQRCode.DAO.Utils
{
    public interface IFrameExtractor
    {
        Task<string[]> ExtractFramesAsync(string videoPath);
    }
}
