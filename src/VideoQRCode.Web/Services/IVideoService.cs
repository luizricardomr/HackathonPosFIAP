namespace VideoQRCode.Web.Services
{
    public interface IVideoService
    {
        Task<bool> EnviarVideoAsync(IFormFile arquivo);
    }
}
