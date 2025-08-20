using Microsoft.OpenApi.Models;
using VideoQRCode.Core.Utils;
using VideoQRCode.DAO.Infra.Repository;
using VideoQRCode.DAO.Services;
using VideoQRCode.DAO.Utils;

namespace VideoQRCode.DAO.Configuration
{
    public static class ApiConfig
    {
        public static IServiceCollection AddApiConfig(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddScoped<IVideoRepository, VideoRepository>();
            services.AddScoped<IConteudoVideoRepository, ConteudoVideoRepository>();
            services.AddScoped<IVideoService, VideoService>();
            services.AddScoped<IConteudoVideoService, ConteudoVideoService>();
            services.AddScoped<IFrameExtractor, FrameExtractor>();
            services.AddScoped<IFrameProcessor, FrameProcessor>();

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


            return services;
        }
    }
}
