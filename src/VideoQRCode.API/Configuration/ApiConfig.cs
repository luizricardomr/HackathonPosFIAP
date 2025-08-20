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
            services.AddSwaggerGen();

            services.AddScoped<IVideoRepository, VideoRepository>();
            services.AddScoped<IVideoProducer, VideoProducer>();
            services.AddScoped<IConteudoVideoRepository, ConteudoVideoRepository>();
            services.AddScoped<IFileStorageService, LocalFileStorageService>();

            return services;
        }
    }
}

