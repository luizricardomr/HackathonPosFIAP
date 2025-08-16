using MassTransit;
using System.Diagnostics.CodeAnalysis;
using VideoQRCode.API.Producers;

namespace VideoQRCode.API.Configuration
{
    [ExcludeFromCodeCoverage]
    public static class RabbitConfiguration
    {
        public static WebApplicationBuilder Configure(this WebApplicationBuilder builder)
        {
            //RabbitMQ
            builder.Services
                .Configure<MassTransitConfig>(builder.Configuration.GetSection("MassTransit"))
                .AddScoped<IVideoProducer, VideoProducer>();

            var config = builder.Configuration;
            var server = config.GetSection("MassTransit")["server"] ?? string.Empty;
            var user = config.GetSection("MassTransit")["user"] ?? string.Empty;
            var password = config.GetSection("MassTransit")["password"] ?? string.Empty;

            builder.Services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(server, "/", h =>
                    {
                        h.Username(user);
                        h.Password(password);
                    });
                    cfg.ConfigureEndpoints(context);
                });
            });

            return builder;
        }
    }
}