namespace VideoQRCode.DAO.Utils
{
    public interface IFrameProcessor
    {
        Task<List<(string Conteudo, string Timestamp)>> ProcessFramesAsync(string[] frames);
    }
}
