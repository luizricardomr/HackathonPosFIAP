using Microsoft.OpenApi.Models;
using VideoQRCode.Core.Utils;
using VideoQRCode.DAO.Infra.Repository;
using VideoQRCode.DAO.Services;
using VideoQRCode.DAO.Utils;

namespace VideoQRCode.DAO.Configuration
{
    public static class ApiConfig
    {
        public static IServiceCollection AddApiConfig(this WebApplicationBuilder builder)
        {
            // Controllers e Swagger
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
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

            // Registro dos serviços internos
            builder.Services.AddScoped<IVideoRepository, VideoRepository>();
            builder.Services.AddScoped<IConteudoVideoRepository, ConteudoVideoRepository>();
            builder.Services.AddScoped<IVideoService, VideoService>();
            builder.Services.AddScoped<IConteudoVideoService, ConteudoVideoService>();
            builder.Services.AddScoped<IFrameExtractor, FrameExtractor>();
            builder.Services.AddScoped<IFrameProcessor, FrameProcessor>();
            builder.Services.AddScoped<INotificacaoService, NotificacaoService>();

            // CORS para frontend local (SignalR/WebSocket)
            var frontendUrl = builder.Configuration["FrontendConfig:Url"]; // ex: "http://localhost:5286"
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins(frontendUrl) 
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            return builder.Services;
        }
    }
}
