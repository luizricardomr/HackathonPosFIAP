using FFMpegCore;

namespace VideoQRCode.DAO.Utils
{
    public class FrameExtractor : IFrameExtractor
    {
        private readonly string _uploadPath;

        public FrameExtractor(IConfiguration config)
        {
            _uploadPath = config["Storage:UploadPath"]
                ?? throw new ArgumentNullException("Storage:UploadPath não configuravel");
        }

        public async Task<string[]> ExtractFramesAsync(string videoPath)
        {
            var frameOutput = Path.Combine(_uploadPath, "frames");
            Directory.CreateDirectory(frameOutput);

            string outputPattern = Path.Combine(frameOutput, "frame_%04d.jpg");

            await FFMpegArguments
                .FromFileInput(videoPath)
                .OutputToFile(outputPattern, true, options => options.WithCustomArgument("-vf fps=1"))
                .ProcessAsynchronously();

            return Directory.GetFiles(frameOutput, "frame_*.jpg");
        }
    }
}