using System.Drawing;
using ZXing.Windows.Compatibility;

namespace VideoQRCode.DAO.Utils
{
    public class FrameProcessor : IFrameProcessor
    {
        public async Task<List<(string Conteudo, string Timestamp)>> ProcessFramesAsync(string[] frames)
        {
            var results = new List<(string Conteudo, string Timestamp)>();
            var barcodeReader = new BarcodeReader();

            foreach (var path in frames)
            {
                try
                {
                    using var bitmap = (Bitmap)Image.FromFile(path);
                    var result = barcodeReader.Decode(bitmap);

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
            }

            return results.OrderBy(r => r.Timestamp).ToList();
        }
    }
}