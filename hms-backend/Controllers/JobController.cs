using hms_backend.DTOs;
using HmsBackend;
using HmsBackend.Models;
using HmsBackend.Services;
using HmsBackend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HmsBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobController(IJobService jobService) : ControllerBase
    {

        private readonly IJobService _jobService = jobService;

        [Authorize(Policy = "HelpDeskOnly")]
        [HttpPost]
        [Route("create-job")]
        public async Task<IActionResult> CreateAJob(CreateJobDto jobCreateRequest)
        {
            try
            {
                var result = await _jobService.CreateAJob(jobCreateRequest);

                return Ok(result);
        }
            catch (Exception ex)
        {
                return BadRequest(ex.Message);
        }

        [Authorize(Roles = "MaintenanceManager,HelpDesk")]
        [Route("view-job-by-id")]
        [HttpGet]
        public async Task<IActionResult> GetJobByIdAsync(string id)
        {
            var job = await _jobService.GetJobByIdAsync(id);
            return Ok(job);
        }

        [Authorize(Roles = "MaintenanceManager")]
        [Route("update-job-users")]
        [HttpPut]
        public async Task<IActionResult> UpdateJobUsers([FromBody]UpdateJobUsersDto updateDto)
        {
            var job = await _jobService.UpdateJobUsersAsync(updateDto);
            return Ok(job);
        }
        [Authorize(Roles = "MaintenanceManager")]
        [HttpPut("update-job-status")]
        public async Task<IActionResult> UpdateJobStatus([FromBody] UpdateJobStatusDto updateDto)
        {
            var updatedJob = await _jobService.UpdateJobStatusAsync(updateDto);
            if (updatedJob == null) return NotFound("Job not found");

            return Ok(updatedJob);
        }

        [HttpGet("dashboard-summary")]
        [Authorize(Roles = "Admin,MaintenanceManager")]
        public async Task<IActionResult> GetDashboardSummary()
        {
            var summary = await _jobService.GetDashboardSummaryAsync();
            return Ok(summary);
                return BadRequest(ex.Message);
            }
        }

    }


}

