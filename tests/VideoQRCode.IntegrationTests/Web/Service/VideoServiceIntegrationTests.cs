using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Moq.Protected;
using System.Net;
using VideoQRCode.Web.Services;

namespace VideoQRCode.IntegrationTests.Web.Service
{
    public class VideoServiceIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public VideoServiceIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }
        [Fact]
        public async Task EnviarVideoAsync_DeveRetornarTrue_QuandoApiResponderSucesso()
        {
            // Arrange
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK
                });

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://localhost")
            };

            var service = new VideoService(httpClient);

            var fileMock = new FormFile(
                baseStream: new MemoryStream(new byte[] { 1, 2, 3 }),
                baseStreamOffset: 0,
                length: 3,
                name: "file",
                fileName: "video.mp4")
            {
                Headers = new HeaderDictionary(),
                ContentType = "video/mp4"
            };

            // Act
            var result = await service.EnviarVideoAsync(fileMock);

            // Assert
            Assert.True(result);
            handlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri!.ToString().EndsWith("video/upload")),
                ItExpr.IsAny<CancellationToken>()
            );
        }
    }
}
