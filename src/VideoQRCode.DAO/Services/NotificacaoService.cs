using Microsoft.AspNetCore.SignalR;
using VideoQRCode.DAO.Hubs;

namespace VideoQRCode.DAO.Services
{
    public class NotificacaoService : INotificacaoService
    {
        private readonly IHubContext<VideoHub> _hubContext;

        public NotificacaoService(IHubContext<VideoHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotificarStatusAsync(string fileName, string status)
        {
            Console.WriteLine($"[Notificação] {fileName} -> {status}");
            await _hubContext.Clients.All.SendAsync("StatusAtualizado", new
            {
                FileName = fileName,
                Status = status
            });
        }
    }
}
