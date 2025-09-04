using MassTransit;
using MassTransit.Testing;
using Moq;
using VideoQRCode.Core.Message;
using VideoQRCode.DAO.Consumers;
using VideoQRCode.DAO.Infra.Repository;
using VideoQRCode.DAO.Services;
using Xunit;

namespace VideoQRCode.IntegrationTests.DAO
{
    public class VideoConsumerIntegrationTests
    {
        [Fact]
        public async Task VideoConsumer_DeveProcessarMensagem()
        {
            var videoServiceMock = new Mock<IVideoService>();
            var videoRepoMock = new Mock<IVideoRepository>();
            var notificacaoMock = new Mock<INotificacaoService>();

            var harness = new InMemoryTestHarness();
            var consumerHarness = harness.Consumer(() => new VideoConsumer(videoServiceMock.Object, videoRepoMock.Object, notificacaoMock.Object));

            await harness.Start();
            try
            {
                var message = new VideoMessage
                {
                    Id = Guid.NewGuid(),
                    FileName = "teste.mp4",
                    Path = "caminho/teste.mp4",
                    UploadedAt = DateTime.UtcNow
                };

                await harness.InputQueueSendEndpoint.Send(message);

                Assert.True(await consumerHarness.Consumed.Any<VideoMessage>());

                notificacaoMock.Verify(n => n.NotificarStatusAsync("teste.mp4", "Processando"), Times.Once);
                videoServiceMock.Verify(s => s.ProcessaVideo(It.IsAny<VideoMessage>()), Times.Once);
                notificacaoMock.Verify(n => n.NotificarStatusAsync("teste.mp4", "Concluído"), Times.Once);
            }
            finally
            {
                await harness.Stop();
            }
        }
    }

}