using MongoDB.Driver;
using VideoQRCode.Core.Domain;

namespace VideoQRCode.DAO.Infra.Repository
{
    public class VideoRepository : IVideoRepository
    {
        private readonly IMongoCollection<Video> _collection;

        public VideoRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<Video>("videos");
        }

        public async Task AddAsync(Video video)
        {
            await _collection.InsertOneAsync(video);
        }


        public async Task<IEnumerable<Video>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task UpdateStatusAsync(Guid id, string status)
        {
            var filter = Builders<Video>.Filter.Eq(v => v.Id, id);
            var update = Builders<Video>.Update.Set(v => v.Status, status);
            await _collection.UpdateOneAsync(filter, update);
        }
    }
}
