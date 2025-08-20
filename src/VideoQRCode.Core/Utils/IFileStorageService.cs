using Microsoft.AspNetCore.Http;

namespace VideoQRCode.Core.Utils
{
    public interface IFileStorageService
    {
        Task<string> SaveFileAsync(IFormFile file, string folder);
    }
}
