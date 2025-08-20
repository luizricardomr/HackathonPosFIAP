using VideoQRCode.Core.Domain;
using VideoQRCode.DAO.Infra.Repository;

namespace VideoQRCode.DAO.Services
{
    public class ConteudoVideoService: IConteudoVideoService
    {
        private readonly IConteudoVideoRepository _repository;

        public ConteudoVideoService(IConteudoVideoRepository repository)
        {
            _repository = repository;
        }
        public async Task SalvarQrCodesAsync(Guid videoId, List<(string Conteudo, string Timestamp)> results)
        {
            var documents = results.Select(item => new ConteudoVideo
            {
                VideoId = videoId,
                Conteudo = item.Conteudo,
                Timestamp = item.Timestamp
            }).ToList();

            await _repository.AddManyAsync(documents);
        }
    }
}
