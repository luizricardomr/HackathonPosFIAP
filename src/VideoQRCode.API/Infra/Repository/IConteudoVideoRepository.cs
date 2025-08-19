using VideoQRCode.API.Domain;

namespace VideoQRCode.API.Infra.Repository
{
    public interface IConteudoVideoRepository
    {
        Task<List<ConteudoVideo>> GetByVideoIdAsync(Guid videoId);
    }
}
