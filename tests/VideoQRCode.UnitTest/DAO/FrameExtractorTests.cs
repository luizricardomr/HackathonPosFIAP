using Microsoft.Extensions.Configuration;
using VideoQRCode.DAO.Utils;

namespace VideoQRCode.UnitTest.DAO
{
    public class FrameExtractorTests
    {
        [Fact]
        public void Construtor_Deve_Lancar_Excecao_Se_Config_Nao_Tiver_UploadPath()
        {
            // Arrange
            var inMemorySettings = new Dictionary<string, string?>
            {
                // "Storage:UploadPath" não definido
            };
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new FrameExtractor(config));
        }
    }
}
