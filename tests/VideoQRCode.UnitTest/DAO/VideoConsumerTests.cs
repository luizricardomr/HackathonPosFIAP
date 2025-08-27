using MassTransit;
using Moq;
using VideoQRCode.Core.Message;
using VideoQRCode.DAO.Consumers;
using VideoQRCode.DAO.Infra.Repository;
using VideoQRCode.DAO.Services;

namespace VideoQRCode.Tests.Consumers
{
    public class VideoConsumerTests
    {
        [Fact]
        public async Task Consume_Deve_Atualizar_Status_E_Processar_Video()
        {
            // Arrange
            var serviceMock = new Mock<IVideoService>();
            var repositoryMock = new Mock<IVideoRepository>();

            var consumer = new VideoConsumer(serviceMock.Object, repositoryMock.Object);

            var mensagem = new VideoMessage
            {
                Id = Guid.NewGuid(),
                Path = "http://teste.com/video.mp4",
                FileName = "video.mp4",
                UploadedAt = DateTime.Now
            };

            var consumeContextMock = new Mock<ConsumeContext<VideoMessage>>();
            consumeContextMock.Setup(c => c.Message).Returns(mensagem);

            // Act
            await consumer.Consume(consumeContextMock.Object);

            // Assert
            repositoryMock.Verify(r => r.UpdateStatusAsync(mensagem.Id, "Processando"), Times.Once);
            serviceMock.Verify(s => s.ProcessaVideo(mensagem), Times.Once);
        }
    }
}
