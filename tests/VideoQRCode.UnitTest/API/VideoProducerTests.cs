using MassTransit;
using Microsoft.Extensions.Options;
using Moq;
using VideoQRCode.API.Configuration;
using VideoQRCode.API.Producers;
using VideoQRCode.Core.Message;

namespace VideoQRCode.Tests.Producers
{
    public class VideoProducerTests
    {
        [Fact]
        public async Task ExecuteAsync_Deve_Enviar_Mensagem_Para_Queue()
        {
            // Arrange
            var busMock = new Mock<IBus>();
            var sendEndpointMock = new Mock<ISendEndpoint>();

            var config = Options.Create(new MassTransitConfig
            {
                VideoQueue = "video-queue"
            });

            var uriEsperada = new Uri("queue:video-queue");

            busMock
                .Setup(b => b.GetSendEndpoint(uriEsperada))
                .ReturnsAsync(sendEndpointMock.Object);

            var producer = new VideoProducer(busMock.Object, config);

            var mensagem = new VideoMessage
            {
                Id = Guid.NewGuid(),
                Path = "http://teste.com/video.mp4",
                FileName = "video.mp4",
                UploadedAt = DateTime.Now
            };

            // Act
            await producer.ExecuteAsync(mensagem);

            // Assert
            sendEndpointMock.Verify(
                s => s.Send(
                    It.Is<VideoMessage>(m => m.Id == mensagem.Id && m.Path == mensagem.Path),
                    default
                ),
                Times.Once
            );
        }
    }
}
