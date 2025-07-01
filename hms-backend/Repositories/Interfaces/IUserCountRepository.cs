namespace HmsBackend.Repositories.Interfaces
{
    public interface IUserCountRepository
    {
        Task<int> GetTotalUserCountAsync();
    }
}