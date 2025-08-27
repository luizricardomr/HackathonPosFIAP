using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using VideoQRCode.API.Infra.Repository;
using VideoQRCode.API.Producers;
using VideoQRCode.Core.Utils;

namespace VideoQRCode.IntegrationTests.Utils
{
    public class IntegrationTestFactory : WebApplicationFactory<Program>
    {
        private readonly Action<IServiceCollection>? _configureServices;

        public IntegrationTestFactory(Action<IServiceCollection>? configureServices = null)
        {
            _configureServices = configureServices;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("IntegrationTest");

            builder.ConfigureServices(services =>
            {
                // Remove implementações reais
                services.RemoveAll<IFileStorageService>();
                services.RemoveAll<IVideoProducer>();
                services.RemoveAll<IVideoRepository>();

                // Adiciona mocks padrão
                services.AddSingleton(Mock.Of<IFileStorageService>());
                services.AddSingleton(Mock.Of<IVideoProducer>());
                services.AddSingleton(Mock.Of<IVideoRepository>());

                // Permite sobrescrever os mocks para cada teste
                _configureServices?.Invoke(services);
            });
        }
    }
}
