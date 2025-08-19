using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VideoQRCode.DAO.Domain
{
    public class ConteudoVideo
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }

        [BsonRepresentation(BsonType.String)]
        public Guid VideoId { get; set; } 

        public string Conteudo { get; set; }

        public string Timestamp { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
