using HmsBackend.DTOs;
using HmsBackend.Models;
using HmsBackend.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HmsBackend.Services
{
    public class JobService(AppDbContext appDbContext, UserManager<User> userManager) : IJobService
    {
        private readonly AppDbContext _context = appDbContext;
        private readonly UserManager<User> _userManager = userManager;

        public async Task<string> CreateAJob(CreateJobDto createJobRequest)
        {
            try
            {
                var complaint = await (from c in _context.Complaints where c.ComplaintNumber == createJobRequest.ComplaintNumber select c).FirstOrDefaultAsync();

                if (complaint == null)
                    throw new Exception($"Cannot find a complaint with a complain number {createJobRequest.ComplaintNumber}");

                var job = new Job
                {
                    JobNumber = $"J{new Random().Next(0, 10000):D4}",
                    Name = complaint.Title,
                    Complaint = complaint,
                    ComplaintId = complaint.Id,
                    Status = "Pending",
                    Description = createJobRequest.Description,
                    Priority = createJobRequest.Priority,
                    IsDeleted = false,
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

        public async Task<List<JobViewDto>> DeleteJob(DeleteJobDto deleteJobReq)
        {
            try
            {
                var job = await (from j in _context.Job where j.JobNumber == deleteJobReq.JobNumber select j).FirstAsync();

                if (job == null)
                    throw new Exception($"Cannot find a job with a job number {deleteJobReq.JobNumber}");

                job.IsDeleted = true;
                await _context.SaveChangesAsync();

                return await GetAllJobsAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Job deletion failed", ex);
            }
        }

        public async Task<List<JobViewDto>> GetAllJobsAsync()
        {
            var jobs = await _context.Job
                .Include(j => j.JobUsers)
                .ThenInclude(ju => ju.User)
                .ToListAsync();

            return jobs.Where(j => j.IsDeleted == false).Select(job => new JobViewDto
            {
                Id = job.Id,
                JobNumber = job.JobNumber,
                Name = job.Name,
                Status = job.Status,
                Description = job.Description,
                Priority = job.Priority,
                CreatedUserId = job.CreatedUserId,
                ComplaintId = job.ComplaintId,
                AssignedManagerUserId = job.AssignedManagerUserId,
                Users = job.JobUsers.Select(ju => new JobUserViewDto
                {
                    UserId = ju.User.Id,
                    FullName = ju.User.FirstName + " " + ju.User.LastName,
                    Role = ju.User.Role
                }).ToList()
            }).ToList();
        }

        public async Task<int> GetAllJobsCount()
        {
            return await _context.Job.CountAsync();
        }

        public async Task<DashboardSummaryDto> GetDashboardSummaryAsync()
        {
            var totalJobs = await _context.Job.CountAsync();
            var completedJobs = await _context.Job
                .Where(j => j.Status == "Completed")
                .CountAsync();

            var totalWorkers = await _userManager.Users
                .Where(u => u.Role == "MaintenanceStaff")
                .CountAsync();

            var activeJobs = await _context.Job
             .Where(j => j.Status == "Pending" || j.Status == "In Progress")
             .CountAsync();

            return new DashboardSummaryDto
            {
                TotalJobs = totalJobs,
                CompletedJobs = completedJobs,
                TotalWorkers = totalWorkers,
                ActiveJobs = activeJobs
            };
        }

        public async Task<JobViewDto> GetJobByIdAsync(string jobId)
        {
            var job = await _context.Job
                .Include(j => j.JobUsers)
                    .ThenInclude(ju => ju.User)
                //.Include(j => j.AssignedManagerUser)
                .FirstOrDefaultAsync(j => j.Id == jobId);

            if (job == null) return null;

            return new JobViewDto
            {
                Id = job.Id,
                Name = job.Name,
                Status = job.Status,
                Description = job.Description,
                Priority = job.Priority,
                CreatedUserId = job.CreatedUserId,
                ComplaintId = job.ComplaintId,
                AssignedManagerUserId = job.AssignedManagerUserId,
                AssignedManagerName = job.AssignedManagerUser != null
                    ? job.AssignedManagerUser.FirstName + " " + job.AssignedManagerUser.LastName
                    : null,
                Users = job.JobUsers.Select(ju => new JobUserViewDto
                {
                    UserId = ju.User.Id,
                    FullName = ju.User.FirstName + " " + ju.User.LastName,
                    Role = ju.User.Role
                }).ToList()
            };
        }

        public async Task<JobViewDto> UpdateJobStatusAsync(UpdateJobStatusDto updateDto)
        {
            var job = await _context.Job
                .Include(j => j.JobUsers)
                .ThenInclude(ju => ju.User)
                .FirstOrDefaultAsync(j => j.Id == updateDto.JobId);

            if (job == null) return null;

            job.Status = updateDto.Status;
            await _context.SaveChangesAsync();

            return new JobViewDto
            {
                Id = job.Id,
                Name = job.Name,
                Status = job.Status,
                Description = job.Description,
                Priority = job.Priority,
                AssignedManagerUserId = job.AssignedManagerUserId,
                CreatedUserId = job.CreatedUserId,
                ComplaintId = job.ComplaintId,
                Users = job.JobUsers.Select(ju => new JobUserViewDto
                {
                    UserId = ju.User.Id,
                    FullName = ju.User.FirstName + " " + ju.User.LastName,
                    Role = ju.User.Role
                }).ToList()
            };
        }

        public async Task<JobViewDto> UpdateJobUsersAsync(UpdateJobUsersDto updateDto)
        {
            var job = await _context.Job
                .Include(j => j.JobUsers)
                .FirstOrDefaultAsync(j => j.Id == updateDto.JobId);

            if (job == null) return null;



            // Create new links
            var newJobUsers = updateDto.UserIds.Select(userId => new JobUser
            {
                JobId = job.Id,
                UserId = userId
            }).ToList();

            job.JobUsers = newJobUsers;
            await _context.SaveChangesAsync();

            // Reload job with users to return updated result
            var updatedJob = await _context.Job
                .Include(j => j.JobUsers)
                .ThenInclude(ju => ju.User)
                .FirstOrDefaultAsync(j => j.Id == job.Id);

            return new JobViewDto
            {
                Id = updatedJob.Id,
                Name = updatedJob.Name,
                Status = updatedJob.Status,
                Description = updatedJob.Description,
                Priority = updatedJob.Priority,
                CreatedUserId = updatedJob.CreatedUserId,
                ComplaintId = updatedJob.ComplaintId,
                AssignedManagerUserId = updatedJob.AssignedManagerUserId,
                Users = updatedJob.JobUsers.Select(ju => new JobUserViewDto
                {
                    UserId = ju.User.Id,
                    FullName = ju.User.FirstName + " " + ju.User.LastName,
                    Role = ju.User.Role
                }).ToList()
            };
        }
    }
}
