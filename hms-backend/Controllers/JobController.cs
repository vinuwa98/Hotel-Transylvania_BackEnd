using hms_backend.DTOs;
using HmsBackend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace hms_backend.Controllers
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
        }

        [Authorize(Policy = "HelpDeskOnly")]
        [HttpPost]
        [Route("delete-job")]
        public async Task<IActionResult> DeleteJob(DeleteJobDto deleteReq)
        {
            try
            {
                var result = await _jobService.DeleteJob(deleteReq);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
