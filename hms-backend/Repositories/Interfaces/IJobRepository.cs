using HmsBackend.Models;

namespace HmsBackend.Repositories.Interfaces
{
    public interface IJobRepository
    {
        Task<IEnumerable<Job>> GetAllAync();
        Task<Job> GetByIdAsync(int id);
        Task AddAsync(Job job);
        Task UpdateAsync(Job job);
        Task DeleteAsync(Job job);


    }
}
