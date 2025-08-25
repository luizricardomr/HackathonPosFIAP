
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Text;
using VideoQRCode.API.Infra.Repository;
using VideoQRCode.API.Producers;
using VideoQRCode.Controllers;
using VideoQRCode.Core.Domain;
using VideoQRCode.Core.Message;
using VideoQRCode.Core.Utils;

namespace VideoQRCode.UnitTest.API
{
    public class VideoControllerTests
    {
        private readonly Mock<IFileStorageService> _fileStorageMock;
        private readonly Mock<IVideoProducer> _producerMock;
        private readonly Mock<IVideoRepository> _videoRepositoryMock;
        private readonly VideoController _controller;

        public VideoControllerTests()
        {
            _fileStorageMock = new Mock<IFileStorageService>();
            _producerMock = new Mock<IVideoProducer>();
            _videoRepositoryMock = new Mock<IVideoRepository>();

            _controller = new VideoController(
                _producerMock.Object,
                _videoRepositoryMock.Object,
                _fileStorageMock.Object
            );
        }
        [Fact]
        public async Task UploadVideo_DeveRetornarBadRequest_QuandoArquivoForNulo()
        {
            // Act
            var result = await _controller.UploadVideo(null);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Nenhum arquivo enviado", badRequest.Value);
        }

        [Fact]
        public async Task UploadVideo_DeveRetornarBadRequest_QuandoArquivoEstiverVazio()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(0);

            // Act
            var result = await _controller.UploadVideo(fileMock.Object);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Nenhum arquivo enviado", badRequest.Value);
        }

        [Fact]
        public async Task UploadVideo_DeveRetornarBadRequest_QuandoArquivoNaoForVideo()
        {
            // Arrange
            var content = "fake data";
            var fileName = "arquivo.txt";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            var fileMock = new FormFile(stream, 0, stream.Length, "Data", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "text/plain"
            };

            // Act
            var result = await _controller.UploadVideo(fileMock);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Formato invalido. Envie um arquivo de video.", badRequest.Value);
        }

        [Fact]
        public async Task UploadVideo_DeveSalvarArquivoEPublicarMensagem_QuandoArquivoForValido()
        {
            // Arrange
            var content = "fake video data";
            var fileName = "video.mp4";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            var fileMock = new FormFile(stream, 0, stream.Length, "Data", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "video/mp4"
            };

            var expectedPath = "/videos/video.mp4";
            _fileStorageMock.Setup(s => s.SaveFileAsync(fileMock, "videos"))
                            .ReturnsAsync(expectedPath);

            // Act
            var result = await _controller.UploadVideo(fileMock);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = okResult.Value.GetType().GetProperty("message")?.GetValue(okResult.Value, null);
            var path = okResult.Value.GetType().GetProperty("path")?.GetValue(okResult.Value, null);

            Assert.Equal("Video enviado", value);
            Assert.Equal(expectedPath, path);

            _fileStorageMock.Verify(s => s.SaveFileAsync(fileMock, "videos"), Times.Once);
            _producerMock.Verify(p => p.ExecuteAsync(It.IsAny<VideoMessage>()), Times.Once);
            _videoRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Video>()), Times.Once);
        }
    }
}    

