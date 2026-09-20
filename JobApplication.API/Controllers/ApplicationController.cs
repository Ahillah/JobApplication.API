using JobApplication.Application.DTO_s.ApplyJobDto;
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
    [Authorize]
    public class ApplicationController : ControllerBase
    {
        private readonly IJobApplicationService _applicationService;

        public ApplicationController(
            IJobApplicationService applicationService)
        {
            _applicationService = applicationService;
        }
        [HttpPost]
        [Authorize(Roles = Roles.Candidate)]
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

        [HttpGet("my-applications")]
        [Authorize(Roles = Roles.Candidate)]
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

        [HttpDelete("{applicationId}")]
        [Authorize(Roles = Roles.Candidate)]
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
        [Authorize(Roles = Roles.Recruiter)]
        [HttpGet("job/{jobId}")]
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
        [Authorize(Roles = Roles.Recruiter)]
        [HttpPatch("{applicationId}/status")]
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
