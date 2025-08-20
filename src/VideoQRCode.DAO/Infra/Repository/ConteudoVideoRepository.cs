using MongoDB.Driver;
using VideoQRCode.Core.Domain;

namespace VideoQRCode.DAO.Infra.Repository
{
    public class ConteudoVideoRepository : IConteudoVideoRepository
    {
        private readonly IMongoCollection<ConteudoVideo> _collection;

        public ConteudoVideoRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<ConteudoVideo>("conteudovideos");
        }

        public async Task AddManyAsync(IEnumerable<ConteudoVideo> results)
        {
            if (results.Any())
                await _collection.InsertManyAsync(results);
        }

        public async Task<IEnumerable<ConteudoVideo>> GetByVideoIdAsync(Guid videoId)
        {
            var filter = Builders<ConteudoVideo>.Filter.Eq(x => x.VideoId, videoId);
            return await _collection.Find(filter).ToListAsync();
        }
    }
}
