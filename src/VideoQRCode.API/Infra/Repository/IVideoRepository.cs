using VideoQRCode.API.Domain;

namespace VideoQRCode.API.Infra.Repository
{
    public interface IVideoRepository
    {
        Task AddAsync(Video video);        
        Task<IEnumerable<Video>> GetAllAsync();
        Task UpdateStatusAsync(Guid id, string status);
        
    }
}
