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
    [Authorize(Roles = Roles.Candidate)]
    public class ApplicationController : ControllerBase
    {
        private readonly IJobApplicationService _applicationService;

        public ApplicationController(
            IJobApplicationService applicationService)
        {
            _applicationService = applicationService;
        }
        [HttpPost]
        public async Task<IActionResult> Apply(
          [FromForm] ApplyForJobDto dto)
        {
            if (!TryGetCandidateId(out var candidateId))
            {
                return Unauthorized(
                    ApiResponse<object>.Failure(
                        "Invalid user identity."));
            }
            await _applicationService.ApplyForJobAsync(
                candidateId,
                dto);
            return StatusCode(
             StatusCodes.Status201Created,
             ApiResponse<object>.Success(
                 null!,
                 "Application submitted successfully."));
        }

        [HttpGet("my-applications")]
        public async Task<IActionResult> GetMyApplications()
        {
            if (!TryGetCandidateId(out var candidateId))
            {
                return Unauthorized(
                    ApiResponse<object>.Failure(
                        "Invalid user identity."));
            }

            var applications =
                await _applicationService
                    .GetMyApplicationsAsync(candidateId);

            return Ok(
                ApiResponse<IReadOnlyList<CandidateApplicationDto>>.Success(
                    applications,
                    "Applications retrieved successfully."));
        }

        [HttpDelete("{applicationId}")]
        public async Task<IActionResult> CancelApplication(
    int applicationId)
        {
            if (!TryGetCandidateId(out var candidateId))
            {
                return Unauthorized(
                    ApiResponse<object>.Failure(
                        "Invalid user identity."));
            }

            await _applicationService.CancelApplicationAsync(
                candidateId,
                applicationId);

            return Ok(
                ApiResponse<object>.Success(
                    null!,
                    "Application cancelled successfully."));
        }
        private bool TryGetCandidateId(out int candidateId)
        {
            var candidateIdClaim =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return int.TryParse(candidateIdClaim, out candidateId);
        }
    }
}
