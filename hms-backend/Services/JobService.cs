using hms_backend.Models;
using hms_backend.Repositories.Interfaces;
using hms_backend.Services.Interfaces;
/*
namespace hms_backend.Services
{
    public class JobService : IJobService
    {
        private readonly IJobRepository _jobRepository;

        public JobService(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }

        public async Task<IEnumerable<Job>> GetAllJobsAsync()
        {
            
            return await _jobRepository.GetAllAsync();
        }

        public async Task<Job> GetJobByIdAsync(int id)
        {
            return await _jobRepository.GetByIdAsync(id);
        }

        public async Task CreateJobAsync(Job job)
        {
           
            await _jobRepository.AddAsync(job);
        }

        public async Task UpdateJobAsync(Job job)
        {
          
            await _jobRepository.UpdateAsync(job);
        }

        public async Task DeleteJobAsync(int id)
        {
            // Check business rules before delete
            await _jobRepository.DeleteAsync(id);
        }
    }
}
*/