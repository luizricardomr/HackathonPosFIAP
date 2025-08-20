using VideoQRCode.Core.Domain;

namespace VideoQRCode.DAO.Infra.Repository
{
    public interface IConteudoVideoRepository
    {
        Task AddManyAsync(IEnumerable<ConteudoVideo> results);
        Task<IEnumerable<ConteudoVideo>> GetByVideoIdAsync(Guid videoId);
    }
}
