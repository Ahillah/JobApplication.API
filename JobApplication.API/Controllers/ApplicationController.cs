using JobApplication.Application.DTO_s.ApplyJobDto;
using JobApplication.Application.Interfaces.IServices;
using JobApplication.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobApplication.API.Controllers
{
    /// <summary>
    /// Manages job applications submitted by candidates
    /// and application status updates performed by recruiters.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ApplicationController : ControllerBase
    {
        private readonly IJobApplicationService _applicationService;

        public ApplicationController(
            IJobApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        /// <summary>
        /// Submits a new application for a job.
        /// </summary>
        /// <param name="dto">
        /// The application data including the job id and candidate CV.
        /// </param>
        /// <returns>
        /// A success response when the application is submitted successfully.
        /// </returns>
        [HttpPost]
        [Authorize(Roles = Roles.Candidate)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Apply(
            [FromForm] ApplyForJobDto dto)
        {
            if (!TryGetUserId(out var userId))
            {
                return Unauthorized(
                    ApiResponse<object>.Failure(
                        "Invalid user identity."));
            }

            await _applicationService.ApplyForJobAsync(
                userId,
                dto);

            return StatusCode(
                StatusCodes.Status201Created,
                ApiResponse<object>.Success(
                    null!,
                    "Application submitted successfully."));
        }

        /// <summary>
        /// Gets all applications submitted by the currently authenticated candidate.
        /// </summary>
        /// <returns>
        /// A list of the candidate's job applications.
        /// </returns>
        [HttpGet("my-applications")]
        [Authorize(Roles = Roles.Candidate)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetMyApplications()
        {
            if (!TryGetUserId(out var userId))
            {
                return Unauthorized(
                    ApiResponse<object>.Failure(
                        "Invalid user identity."));
            }

            var applications =
                await _applicationService
                    .GetMyApplicationsAsync(userId);

            return Ok(
                ApiResponse<IReadOnlyList<CandidateApplicationDto>>.Success(
                    applications,
                    "Applications retrieved successfully."));
        }

        /// <summary>
        /// Cancels an existing application submitted by the current candidate.
        /// </summary>
        /// <param name="applicationId">
        /// The unique identifier of the application to cancel.
        /// </param>
        /// <returns>
        /// A success response when the application is cancelled successfully.
        /// </returns>
        [HttpDelete("{applicationId}")]
        [Authorize(Roles = Roles.Candidate)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CancelApplication(
            int applicationId)
        {
            if (!TryGetUserId(out var userId))
            {
                return Unauthorized(
                    ApiResponse<object>.Failure(
                        "Invalid user identity."));
            }

            await _applicationService.CancelApplicationAsync(
                userId,
                applicationId);

            return Ok(
                ApiResponse<object>.Success(
                    null!,
                    "Application cancelled successfully."));
        }

        /// <summary>
        /// Gets all applications submitted for a specific job.
        /// </summary>
        /// <param name="jobId">
        /// The unique identifier of the job.
        /// </param>
        /// <returns>
        /// A list of applications submitted for the specified job.
        /// </returns>
        [Authorize(Roles = Roles.Recruiter)]
        [HttpGet("job/{jobId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetJobApplications(
            int jobId)
        {
            if (!TryGetUserId(out var recruiterId))
            {
                return Unauthorized(
                    ApiResponse<object>.Failure(
                        "Invalid user identity."));
            }

            var applications =
                await _applicationService
                    .GetJobApplicationsAsync(
                        recruiterId,
                        jobId);

            return Ok(
                ApiResponse<IReadOnlyList<JobApplicationDetailsDto>>.Success(
                    applications,
                    "Job applications retrieved successfully."));
        }

        /// <summary>
        /// Updates the status of an existing job application.
        /// </summary>
        /// <param name="applicationId">
        /// The unique identifier of the application.
        /// </param>
        /// <param name="dto">
        /// The new application status.
        /// </param>
        /// <returns>
        /// A success response when the application status is updated successfully.
        /// </returns>
        [Authorize(Roles = Roles.Recruiter)]
        [HttpPatch("{applicationId}/status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateApplicationStatus(
            int applicationId,
            [FromBody] UpdateApplicationStatusDto dto)
        {
            if (!TryGetUserId(out var recruiterId))
            {
                return Unauthorized(
                    ApiResponse<object>.Failure(
                        "Invalid user identity."));
            }

            await _applicationService.UpdateApplicationStatusAsync(
                recruiterId,
                applicationId,
                dto);

            return Ok(
                ApiResponse<object>.Success(
                    null!,
                    "Application status updated successfully."));
        }

        private bool TryGetUserId(out int userId)
        {
            var userIdClaim =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return int.TryParse(userIdClaim, out userId);
        }
    }
}