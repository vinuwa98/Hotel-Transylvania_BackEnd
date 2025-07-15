using HmsBackend.DTOs;
using HmsBackend.Models;
using HmsBackend.Repositories.Interfaces;
using HmsBackend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HmsBackend.Services
{
    public class JobService(AppDbContext appDbContext) : IJobService
    {
        private readonly AppDbContext _context = appDbContext;

        public async Task<string> CreateAJob(CreateJobDto createJobRequest)
        {
            try
            {
                var complaint = await (from c in _context.Complaint where c.ComplaintNumber == createJobRequest.ComplaintNumber select c).FirstOrDefaultAsync();

                if (complaint == null)
                    throw new Exception($"Cannot find a complaint with a complain number {createJobRequest.ComplaintNumber}");

                var job = new Job
                {
                    JobNumber = $"J",
                    Name = complaint.Title,
                    Complaint = complaint,
                    ComplaintId = complaint.Id,
                    Status = "Pending",
                    Description = complaint.Description,
                    Priority = createJobRequest.Priority,
                    IsDeleted = true,
                };

                await _context.Job.AddAsync(job);
                await _context.SaveChangesAsync();

                return "Job created successfully";
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Job creation failed", ex);
            }
        }

        public async Task<string> DeleteJob(DeleteJobDto deleteJobReq)
        {
            try
            {
                var job = await (from j in _context.Job where j.JobNumber == deleteJobReq.JobNumber select j).FirstAsync();

                if (job == null)
                    throw new Exception($"Cannot find a job with a job number {deleteJobReq.JobNumber}");

                job.IsDeleted = true;
                await _context.SaveChangesAsync();

                return "Job delete successfully!";
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Job deletion failed", ex);
            }
        }
    }
}
