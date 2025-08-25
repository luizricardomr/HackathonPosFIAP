using SkiaSharp;
using System.Collections.Concurrent;
using ZXing.SkiaSharp;

namespace VideoQRCode.DAO.Utils
{
    public class FrameProcessor : IFrameProcessor
    {
        public async Task<List<(string Conteudo, string Timestamp)>> ProcessFramesAsync(string[] frames)
        {
            var results = new ConcurrentBag<(string Conteudo, string Timestamp)>();           

            await Parallel.ForEachAsync(frames, async (path, token) =>
            {
                try
                {
                    var barcodeReader = new BarcodeReader(); 
                    using var bitmap = SKBitmap.Decode(path);
                    var source = new ZXing.SkiaSharp.SKBitmapLuminanceSource(bitmap);
                    var result = barcodeReader.Decode(source);

                    if (result != null)
                    {
                        var fileName = Path.GetFileNameWithoutExtension(path);
                        results.Add((result.Text, fileName));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro processando frame {path}: {ex.Message}");
                }

                await Task.Yield();
            });

            return results.OrderBy(r => r.Timestamp).ToList();
        }
    }
}
