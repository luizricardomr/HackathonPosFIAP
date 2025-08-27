using System.Diagnostics.CodeAnalysis;
using VideoQRCode.API.Producers;
using MassTransit;

namespace VideoQRCode.API.Configuration
{
    [ExcludeFromCodeCoverage]
    public static class RabbitConfiguration
    {
        public static WebApplicationBuilder Configure(this WebApplicationBuilder builder)
        {
            // Configuração básica
            builder.Services
                .Configure<MassTransitConfig>(builder.Configuration.GetSection("MassTransit"))
                .AddScoped<IVideoProducer, VideoProducer>();

            var config = builder.Configuration;
            var server = config.GetSection("MassTransit")["server"] ?? string.Empty;
            var user = config.GetSection("MassTransit")["user"] ?? string.Empty;
            var password = config.GetSection("MassTransit")["password"] ?? string.Empty;

            // Condicional de ambiente
            if (!builder.Environment.IsEnvironment("IntegrationTest"))
            {
                // Produção / desenvolvimento normal: RabbitMQ
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
            }
            else
            {
                // Ambiente de teste: transporte em memória, sem EventLog
                builder.Services.AddMassTransit(x =>
                {
                    x.UsingInMemory((context, cfg) =>
                    {
                        cfg.ConfigureEndpoints(context);
                    });
                });
            }

            return builder;
        }
    }
}
