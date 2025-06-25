using HmsBackend.Models;

namespace HmsBackend.Services.Interfaces
{
    public interface IJobService
    {
        Task<IEnumerable<Job>> GetAllJobsAsync();
        Task<Job> GetJobByIdAsync(int id);
        Task CreateJobAsync(Job job);
        Task UpdateJobAsync(Job job);
        Task DeleteJobAsync(int id);
    }
}
