namespace VideoQRCode.Core.Message
{
    public class VideoMessage
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}
