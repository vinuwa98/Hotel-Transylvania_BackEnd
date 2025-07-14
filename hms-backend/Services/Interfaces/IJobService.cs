using hms_backend.DTOs;
using HmsBackend.DTOs;
using HmsBackend.Models;
using Microsoft.AspNetCore.Mvc;

namespace HmsBackend.Services.Interfaces
{
    public interface IJobService
    {
        //Task<IEnumerable<Job>> GetAllJobsAsync();
        //Task<Job> GetJobByIdAsync(int id);
        //Task CreateJobAsync(Job job);
        Task<string> CreateAJob(CreateJobDto createJobRequest);
        //Task UpdateJobAsync(Job job);
        //Task DeleteJobAsync(int id);
        Task<string> DeleteJob(DeleteJobDto deleteJobReq);
    }
}
