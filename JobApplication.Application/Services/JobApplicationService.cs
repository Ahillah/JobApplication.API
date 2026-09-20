using JobApplication.Application.DTO_s.ApplyJobDto;
using JobApplication.Application.Interfaces.IRepositories;
using JobApplication.Application.Interfaces.IServices;
using JobApplication.Domain.Entities;
using JobApplication.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplication.Application.Services
{
    public class JobApplicationService : IJobApplicationService
    {
        private readonly IJobRepository _jobRepository;
        private readonly IJobCandidateApplicationRepository _applicationRepository;
        private readonly IStorageService _storageService;

        public JobApplicationService(
            IJobRepository jobRepository,
            IJobCandidateApplicationRepository applicationRepository,
            IStorageService storageService)
        {
            _jobRepository = jobRepository;
            _applicationRepository = applicationRepository;
            _storageService = storageService;
        }

        public async Task ApplyForJobAsync(
            int candidateId,
            ApplyForJobDto dto)
        {
            var job = await _jobRepository.GetByIdAsync(dto.JobId);

            if (job is null)
            {
                throw new KeyNotFoundException(
                    "Job was not found.");
            }

            if (!job.IsActive)
            {
                throw new InvalidOperationException(
                    "You can only apply for active jobs.");
            }
            if (job.IsClosed)
            {
                throw new InvalidOperationException(
                    "This job is closed and no longer accepts applications.");
            }

            var alreadyApplied =
                await _applicationRepository
                    .ExistsForCandidateAsync(
                        candidateId,
                        dto.JobId);

            if (alreadyApplied)
            {
                throw new InvalidOperationException(
                    "You have already applied for this job.");
            }

         
            var cvUrl = await _storageService
                .UploadFileAsync(dto.Cv);

         
            var application = new JobCandidateApplication
            {
                ApplicationUserId = candidateId,
                JobId = dto.JobId,
                CvUrl = cvUrl,

             
                JobApplicationStatus = JobApplicationStatus.Applied,

                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

           
            await _applicationRepository.AddAsync(application);
            await _applicationRepository.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<CandidateApplicationDto>>
    GetMyApplicationsAsync(int candidateId)
        {
            var applications =
                await _applicationRepository
                    .GetByCandidateIdAsync(candidateId);

            return applications
                .Select(application => new CandidateApplicationDto
                {
                    Id = application.Id,
                    JobId = application.JobId,
                    JobTitle = application.Job.Title,
                    JobApplicationStatus =
                        application.JobApplicationStatus.ToString(),
                    CvUrl = application.CvUrl,
                    AppliedAt = application.CreatedAt
                })
                .ToList();
        }

        public async Task CancelApplicationAsync(
    int candidateId,
    int applicationId)
        {
            var application =
                await _applicationRepository.GetByIdAsync(
                    applicationId,
                    trackChanges: true);

            if (application is null)
            {
                throw new KeyNotFoundException(
                    "Application was not found.");
            }

            if (application.ApplicationUserId != candidateId)
            {
                throw new UnauthorizedAccessException(
                    "You are not allowed to cancel this application.");
            }

            if (application.JobApplicationStatus
                == JobApplicationStatus.Cancelled)
            {
                throw new InvalidOperationException(
                    "This application has already been cancelled.");
            }

            if (application.JobApplicationStatus != JobApplicationStatus.Applied &&
                application.JobApplicationStatus != JobApplicationStatus.UnderReview)
            {
                throw new InvalidOperationException(
                    "This application cannot be cancelled at its current status.");
            }

            application.JobApplicationStatus =
                JobApplicationStatus.Cancelled;

            application.CancelledAt = DateTime.UtcNow;

            application.UpdatedAt = DateTime.UtcNow;

            await _applicationRepository.SaveChangesAsync();
        }
    }
}
