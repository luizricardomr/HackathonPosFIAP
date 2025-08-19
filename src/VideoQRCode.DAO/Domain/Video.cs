using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using VideoQRCode.Core;

namespace VideoQRCode.DAO.Domain
{
    public class Video
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)] 
        public Guid Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Video(VideoMessage message, string status)
        {
            Id = message.Id;
            FileName = message.FileName;
            Path = message.Path;
            CreatedAt = message.UploadedAt;
            Status = status;
        }
    }
}
