using System.Net.Http.Headers;

namespace VideoQRCode.Web.Services
{
    public class VideoService: IVideoService
    {
        private readonly HttpClient _httpClient;

        public VideoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> EnviarVideoAsync(IFormFile arquivo)
        {
            if (arquivo == null || arquivo.Length == 0)
                return false;

            using var content = new MultipartFormDataContent();
            using var stream = arquivo.OpenReadStream();
            var streamContent = new StreamContent(stream);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(arquivo.ContentType);

            content.Add(streamContent, "file", arquivo.FileName);


            var resposta = await _httpClient.PostAsync("video/upload", content);

            return resposta.IsSuccessStatusCode;
        }
    }
}
