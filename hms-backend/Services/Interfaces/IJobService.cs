using HmsBackend.DTOs;
using HmsBackend.Models;

namespace HmsBackend.Services.Interfaces
{
    public interface IJobService
    {
        Task<int> GetAllJobsCount();
        Task<List<JobViewDto>> GetAllJobsAsync();
        Task<JobViewDto> GetJobByIdAsync(string jobId);
        Task<JobViewDto> UpdateJobUsersAsync(UpdateJobUsersDto updateDto);
        Task<JobViewDto> UpdateJobStatusAsync(UpdateJobStatusDto updateDto);
        Task<DashboardSummaryDto> GetDashboardSummaryAsync();
        Task<string> CreateAJob(CreateJobDto createJobRequest);
        Task<List<JobViewDto>> DeleteJob(DeleteJobDto deleteJobReq);
    }
}
