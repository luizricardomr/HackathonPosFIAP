using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace VideoQRCode.Core.Utils
{
    public class LocalFileStorageService : IFileStorageService
    {
        private readonly IConfiguration _config;

        public LocalFileStorageService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<string> SaveFileAsync(IFormFile file, string folder)
        {
            var uploadsDir = _config["Storage:UploadPath"];
            if (!Directory.Exists(uploadsDir))
                Directory.CreateDirectory(uploadsDir);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var fullPath = Path.Combine(uploadsDir, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fullPath;
        }
    }
}
