namespace VideoQRCode.DAO.Infra.Repository
{
    public interface IVideoRepository
    {
        Task UpdateStatusAsync(Guid id, string status);
    }
}
