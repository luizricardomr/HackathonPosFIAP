using MassTransit;
using MassTransit.Middleware;
using VideoQRCode.DAO.Consumers;

namespace VideoQRCode.DAO.Configuration
{
    public static class RabbitConfiguration
    {
        public static WebApplicationBuilder Configure(this WebApplicationBuilder builder)
        {
            //RabbitMQ
            builder.Services
                .Configure<MassTransitConfig>(builder.Configuration.GetSection("MassTransit"));

            var config = builder.Configuration;
            var server = config.GetSection("MassTransit")["server"] ?? string.Empty;
            var user = config.GetSection("MassTransit")["user"] ?? string.Empty;
            var password = config.GetSection("MassTransit")["password"] ?? string.Empty;
            var videoQueue = config.GetSection("MassTransit")["videoQueue"] ?? string.Empty;

            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumer<VideoConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(server, "/", h =>
                    {
                        h.Username(user);
                        h.Password(password);
                    });
                    cfg.ReceiveEndpoint(videoQueue, e =>
                    {
                        e.ConfigureConsumeTopology = false;
                        e.Consumer<VideoConsumer>(context);

                        e.ConfigureDeadLetter(x =>
                        {
                            x.UseFilter(new DeadLetterTransportFilter());
                        });
                    });

                });
            });
            return builder;
        }
    }
}
