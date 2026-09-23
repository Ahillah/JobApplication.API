using JobApplication.Application.DTO_s.JobDto;
using JobApplication.Application.Features.CloseJob.Commands.UpdateJobToBeClosed;
using JobApplication.Application.Interfaces.IServices;
using JobApplication.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobApplication.API.Controllers
{
    /// <summary>
    /// Manages job postings, including creation, updates, status changes,
    /// retrieving job details, and closing jobs.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class JobsController : ControllerBase
    {
        private readonly IJobService _jobService;
        private readonly IMediator _mediator;
  
     
        public JobsController(IJobService jobService , IMediator mediator)
            {
                _jobService = jobService;
              _mediator = mediator;
            }

        /// <summary>
        /// Creates a new job posting.
        /// </summary>
        /// <param name="dto">
        /// The job information including title and description.
        /// </param>
        /// <returns>
        /// A success response when the job is created successfully.
        /// </returns>
        [Authorize(Roles = Roles.Recruiter)]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

        /// <summary>
        /// Updates an existing job posting.
        /// </summary>
        /// <param name="jobId">
        /// The unique identifier of the job.
        /// </param>
        /// <param name="dto">
        /// The updated job information.
        /// </param>
        /// <returns>
        /// A success response when the job is updated successfully.
        /// </returns>
        [Authorize(Roles = Roles.Recruiter)]
        [HttpPut("{jobId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Updates the active status of a job posting.
        /// </summary>
        /// <param name="jobId">
        /// The unique identifier of the job.
        /// </param>
        /// <param name="dto">
        /// The new active status of the job.
        /// </param>
        /// <returns>
        /// A success response when the job status is updated successfully.
        /// </returns>
        [Authorize(Roles = Roles.Recruiter)]
        [HttpPatch("{jobId}/status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Gets the details of a specific job posting.
        /// </summary>
        /// <param name="jobId">
        /// The unique identifier of the job.
        /// </param>
        /// <returns>
        /// The details of the requested job.
        /// </returns>
        [Authorize]
        [HttpGet("{jobId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Closes a job posting and prevents new applications.
        /// </summary>
        /// <param name="jobId">
        /// The unique identifier of the job to close.
        /// </param>
        /// <returns>
        /// A success response when the job is closed successfully.
        /// </returns>
        [Authorize(Roles = Roles.Recruiter)]
        [HttpPost("{jobId}/close")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CloseJob(
            int jobId)
        {
            if (!TryGetUserId(out var recruiterId))
            {
                return Unauthorized(
                    ApiResponse<object>.Failure(
                        "Invalid user identity."));
            }

            await _mediator.Send(new CloseJobCommand
            {
                JobId = jobId,
                RecruiterId = recruiterId
            });
         
            await _mediator.Send(new CloseJobCommand()
            {
                JobId = jobId,
                RecruiterId = recruiterId
            });

            return Ok(
                ApiResponse<object>.Success(
                    null!,
                    "Job closed successfully."));
        }

        private bool TryGetUserId(out int userId)
        {
            var userIdClaim =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return int.TryParse(userIdClaim, out userId);
        }
    }
}