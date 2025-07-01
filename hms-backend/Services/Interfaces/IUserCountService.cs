namespace HmsBackend.Services.Interfaces
{
    public interface IUserCountService
    {
        Task<int> GetUserCountAsync();
    }
}