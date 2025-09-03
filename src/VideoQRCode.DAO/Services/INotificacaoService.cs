namespace VideoQRCode.DAO.Services
{
    public interface INotificacaoService
    {
        Task NotificarStatusAsync(string filename, string status);
    }
}
