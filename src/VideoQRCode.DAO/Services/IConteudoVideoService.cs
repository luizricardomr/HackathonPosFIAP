namespace VideoQRCode.DAO.Services
{
    public interface IConteudoVideoService
    {
        Task SalvarQrCodesAsync(Guid videoId, List<(string Conteudo, string Timestamp)> results);
    }
}
