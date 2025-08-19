using MongoDB.Driver;
using VideoQRCode.API.Domain;

namespace VideoQRCode.API.Infra.Repository
{
    public class ConteudoVideoRepository : IConteudoVideoRepository
    {
        private readonly IMongoCollection<ConteudoVideo> _collection;

        public ConteudoVideoRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<ConteudoVideo>("conteudovideos");
        }

        public async Task<List<ConteudoVideo>> GetByVideoIdAsync(Guid videoId)
        {
            return await _collection.Find(c => c.VideoId == videoId).ToListAsync();
        }
    }
}
