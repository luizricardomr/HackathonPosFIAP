using Microsoft.OpenApi.Models;
using VideoQRCode.API.Infra.Repository;
using VideoQRCode.API.Producers;
using VideoQRCode.Core.Utils;

namespace VideoQRCode.API.Configuration
{
    public static class ApiConfig
    {
        public static IServiceCollection AddApiConfig(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Video QRCODE API",
                    Version = "v1",
                    Description = "HACKATHON realizado pelo grupo 13",
                    License = new OpenApiLicense
                    {
                        Name = "MIT",
                        Url = new Uri("https://opensource.org/licenses/MIT")
                    }
                });
            });

            services.AddScoped<IVideoRepository, VideoRepository>();
            services.AddScoped<IVideoProducer, VideoProducer>();
            services.AddScoped<IConteudoVideoRepository, ConteudoVideoRepository>();
            services.AddScoped<IFileStorageService, LocalFileStorageService>();

            return services;
        }
    }
}

