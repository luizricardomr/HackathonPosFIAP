
using FFMpegCore;
using System.Collections.Concurrent;
using System.Drawing;
using VideoQRCode.Core;
using ZXing.Windows.Compatibility;

namespace VideoQRCode.DAO.Services
{
    public class VideoService : IVideoService
    {
        private readonly IConfiguration _config;
        public VideoService(IConfiguration config)
        { 
            _config = config;
        }
        public async Task ProcessaVideo(VideoMessage message)
        {
            var results = new ConcurrentBag<(string Conteudo, string Timestamp)>();

            var uploadsDir = _config["Storage:UploadPath"];

            var tempFramesDir = Path.Combine(uploadsDir, Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempFramesDir);

            // Extrair frames a cada 1 segundo usando FFMpeg
            await FFMpegArguments.FromFileInput(message.Path)
                .OutputToFile(Path.Combine(tempFramesDir, "frame_%04d.png"), true, options =>
                    options.WithCustomArgument("-vf fps=1"))
                .ProcessAsynchronously();

            // Pega os frames extraídos e garante que estão ordenados
            var frames = Directory.GetFiles(tempFramesDir, "*.png")
                                  .OrderBy(f => f, StringComparer.OrdinalIgnoreCase)
                                  .ToArray();

            // Processa os frames em paralelo
            Parallel.ForEach(
                frames.Select((path, index) => new { Path = path, Index = index }),
                new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount },
                frame =>
                {
                    var localReader = new BarcodeReader();

                    using (var bitmap = (Bitmap)Image.FromFile(frame.Path))
                    {
                        var result = localReader.Decode(bitmap);
                        if (result != null)
                        {
                            var timestamp = TimeSpan.FromSeconds(frame.Index).ToString(@"hh\:mm\:ss");
                            results.Add((result.Text, timestamp));
                        }
                    }
                }
            );

            var orderedResults = results.OrderBy(r => r.Timestamp).ToList();

            // Salvar resultados em TXT
            var txtPath = Path.Combine(uploadsDir, "qrcodes_detectados.txt");
            using (var writer = new StreamWriter(txtPath))
            {
                foreach (var item in orderedResults)
                    writer.WriteLine($"{item.Timestamp} - {item.Conteudo}");
            }
        }
    }
}
