using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using VideoQRCode.API.Infra.Repository;
using VideoQRCode.API.Producers;
using VideoQRCode.Core.Domain;
using VideoQRCode.Core.Message;
using VideoQRCode.Core.Utils;
using VideoQRCode.IntegrationTests.Utils;

namespace VideoQRCode.IntegrationTests.Api
{
    public class ConteudoVideoControllerTests
    {
        [Fact]
        public async Task Deve_FazerUpload_ComSucesso()
        {
            // Arrange: mock do IFileStorage
            var fileStorageMock = new Mock<IFileStorageService>();
            fileStorageMock
                .Setup(x => x.SaveFileAsync(It.IsAny<IFormFile>(), It.IsAny<string>()))
                .ReturnsAsync("videos/fakepath/video.mp4");

            await using var factory = new IntegrationTestFactory(services =>
            {
                services.AddSingleton(fileStorageMock.Object);
            });

            var client = factory.CreateClient();

            using var fileContent = new ByteArrayContent(new byte[] { 1, 2, 3 });
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("video/mp4");

            var multipartContent = new MultipartFormDataContent
        {
            { fileContent, "file", "video.mp4" }
        };

            // Act
            var response = await client.PostAsync("/video/upload", multipartContent);

            // Assert
            response.EnsureSuccessStatusCode();
            fileStorageMock.Verify(
                x => x.SaveFileAsync(It.IsAny<IFormFile>(), "videos"),
                Times.Once
            );
        }
        [Fact]
        public async Task UploadVideo_DeveRetornarBadRequest_QuandoNenhumArquivo()
        {
            // Arrange
            await using var factory = new IntegrationTestFactory();
            var client = factory.CreateClient();

            var multipartContent = new MultipartFormDataContent(); // sem arquivo

            // Act
            var response = await client.PostAsync("/video/upload", multipartContent);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task UploadVideo_DeveRetornarBadRequest_QuandoTipoInvalido()
        {
            // Arrange
            await using var factory = new IntegrationTestFactory();
            var client = factory.CreateClient();

            using var fileContent = new ByteArrayContent(new byte[] { 1, 2, 3 });
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png"); // tipo inválido

            var multipartContent = new MultipartFormDataContent
            {
                { fileContent, "file", "imagem.png" }
            };

            // Act
            var response = await client.PostAsync("/video/upload", multipartContent);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);

            var message = await response.Content.ReadAsStringAsync();
            Assert.Contains("Formato invalido", message);
        }


    }
}