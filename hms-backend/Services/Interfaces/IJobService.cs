using hms_backend.DTOs;
using HmsBackend.Models;

namespace HmsBackend.Services.Interfaces
{
    public interface IJobService
    {
      /*
        Task<Job> GetJobByIdAsync(int id);
        Task CreateJobAsync(Job job);
        Task UpdateJobAsync(Job job);
        Task DeleteJobAsync(int id);*/

        Task<int> GetAllJobsCount();
        Task<List<JobViewDto>> GetAllJobsAsync();
        Task<JobViewDto> GetJobByIdAsync(string jobId);

        Task<JobViewDto> UpdateJobUsersAsync(UpdateJobUsersDto updateDto);

        Task<JobViewDto> UpdateJobStatusAsync(UpdateJobStatusDto updateDto);

        Task<DashboardSummaryDto> GetDashboardSummaryAsync();


    }
}
