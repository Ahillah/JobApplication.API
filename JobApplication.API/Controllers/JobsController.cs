using JobApplication.Application.DTO_s.JobDto;
using JobApplication.Application.Interfaces.IServices;
using JobApplication.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobApplication.API.Controllers
{
  
        [ApiController]
        [Route("api/[controller]")]
        public class JobsController : ControllerBase
        {
            private readonly IJobService _jobService;

            public JobsController(IJobService jobService)
            {
                _jobService = jobService;
            }

            [Authorize(Roles = Roles.Recruiter)]
            [HttpPost]
            public async Task<IActionResult> CreateJob(
                [FromBody] CreateJobDto dto)
            {
                if (!TryGetUserId(out var recruiterId))
                {
                    return Unauthorized(
                        ApiResponse<object>.Failure(
                            "Invalid user identity."));
                }

                await _jobService.CreateJobAsync(
                    recruiterId,
                    dto);

                return StatusCode(
                    StatusCodes.Status201Created,
                    ApiResponse<object>.Success(
                        null!,
                        "Job created successfully."));
            }

            [Authorize(Roles = Roles.Recruiter)]
            [HttpPut("{jobId}")]
            public async Task<IActionResult> UpdateJob(
                int jobId,
                [FromBody] UpdateJobDto dto)
            {
                if (!TryGetUserId(out var recruiterId))
                {
                    return Unauthorized(
                        ApiResponse<object>.Failure(
                            "Invalid user identity."));
                }

                await _jobService.UpdateJobAsync(
                    recruiterId,
                    jobId,
                    dto);

                return Ok(
                    ApiResponse<object>.Success(
                        null!,
                        "Job updated successfully."));
            }

            [Authorize(Roles = Roles.Recruiter)]
            [HttpPatch("{jobId}/status")]
            public async Task<IActionResult> UpdateJobStatus(
                int jobId,
                [FromBody] UpdateJobStatusDto dto)
            {
                if (!TryGetUserId(out var recruiterId))
                {
                    return Unauthorized(
                        ApiResponse<object>.Failure(
                            "Invalid user identity."));
                }

                await _jobService.UpdateJobStatusAsync(
                    recruiterId,
                    jobId,
                    dto);

                return Ok(
                    ApiResponse<object>.Success(
                        null!,
                        "Job status updated successfully."));
            }

            [Authorize]
            [HttpGet("{jobId}")]
            public async Task<IActionResult> GetJobDetails(
                int jobId)
            {
                var job =
                    await _jobService.GetJobDetailsAsync(jobId);

                return Ok(
                    ApiResponse<JobDetailsDto>.Success(
                        job,
                        "Job details retrieved successfully."));
            }

            private bool TryGetUserId(out int userId)
            {
                var userIdClaim =
                    User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                return int.TryParse(userIdClaim, out userId);
            }
        }
    }


