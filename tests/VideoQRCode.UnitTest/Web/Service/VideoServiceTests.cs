using Microsoft.AspNetCore.Http;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;
using VideoQRCode.Web.Services;

namespace VideoQRCode.UnitTest.Web.Service
{
    public class VideoServiceTests
    {
        [Fact]
        public async Task EnviarVideoAsync_DeveRetornarFalse_SeArquivoForNulo()
        {
            // Arrange
            var httpClient = new HttpClient(new Mock<HttpMessageHandler>().Object);
            var service = new VideoService(httpClient);

            // Act
            var resultado = await service.EnviarVideoAsync(null);

            // Assert
            Assert.False(resultado);
        }

        [Fact]
        public async Task EnviarVideoAsync_DeveRetornarFalse_SeArquivoVazio()
        {
            // Arrange
            var httpClient = new HttpClient(new Mock<HttpMessageHandler>().Object);
            var service = new VideoService(httpClient);

            var fileMock = new FormFile(Stream.Null, 0, 0, "file", "video.mp4");

            // Act
            var resultado = await service.EnviarVideoAsync(fileMock);

            // Assert
            Assert.False(resultado);
        }

        [Fact]
        public async Task EnviarVideoAsync_DeveRetornarTrue_SeUploadComSucesso()
        {
            // Arrange
            var handlerMock = new Mock<HttpMessageHandler>();

            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK
                });

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://fakeapi.com/")
            };

            var service = new VideoService(httpClient);

            var bytes = Encoding.UTF8.GetBytes("fake file content");
            var stream = new MemoryStream(bytes);
            var fileMock = new FormFile(stream, 0, stream.Length, "file", "video.mp4")
            {
                Headers = new HeaderDictionary(),
                ContentType = "video/mp4"
            };

            // Act
            var resultado = await service.EnviarVideoAsync(fileMock);

            // Assert
            Assert.True(resultado);
        }
    }
}
