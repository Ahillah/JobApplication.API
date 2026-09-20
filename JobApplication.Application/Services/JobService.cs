using JobApplication.Application.DTO_s.JobDto;
using JobApplication.Application.Interfaces.IRepositories;
using JobApplication.Application.Interfaces.IServices;
using JobApplication.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplication.Application.Services
{
    public class JobService : IJobService
    {
        private readonly IJobRepository _jobRepository;

        public JobService(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }
        public async Task CreateJobAsync(int recruiterId, CreateJobDto dto)
        {
            var job = new Job
            {
                Title = dto.Title,
                Description = dto.Description,
                RecruiterId = recruiterId,
                IsActive = true,
                IsClosed = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _jobRepository.AddAsync(job);
            await _jobRepository.SaveChangesAsync();
        }
        public async Task UpdateJobAsync(
          int recruiterId,
          int jobId,
          UpdateJobDto dto)
        {
            var job = await _jobRepository.GetByIdAsync(
                jobId,
                trackChanges: true);

            if (job is null)
            {
                throw new KeyNotFoundException(
                    "Job was not found.");
            }

            if (job.RecruiterId != recruiterId)
            {
                throw new UnauthorizedAccessException(
                    "You are not allowed to modify this job.");
            }

            job.Title = dto.Title;
            job.Description = dto.Description;
            job.UpdatedAt = DateTime.UtcNow;

            await _jobRepository.SaveChangesAsync();
        }
        public async Task UpdateJobStatusAsync(
          int recruiterId,
          int jobId,
          UpdateJobStatusDto dto)
        {
            var job = await _jobRepository.GetByIdAsync(
                jobId,
                trackChanges: true);

            if (job is null)
            {
                throw new KeyNotFoundException(
                    "Job was not found.");
            }

            if (job.RecruiterId != recruiterId)
            {
                throw new UnauthorizedAccessException(
                    "You are not allowed to modify this job.");
            }

            job.IsActive = dto.IsActive;
            job.UpdatedAt = DateTime.UtcNow;

            await _jobRepository.SaveChangesAsync();
        }
        public async Task<JobDetailsDto> GetJobDetailsAsync(
           int jobId)
        {
            var job = await _jobRepository.GetByIdAsync(jobId);

            if (job is null)
            {
                throw new KeyNotFoundException(
                    "Job was not found.");
            }

            return new JobDetailsDto
            {
                Id = job.Id,
                Title = job.Title,
                Description = job.Description,
                IsActive = job.IsActive,
                IsClosed = job.IsClosed,
                CreatedAt = job.CreatedAt
            };
        }
        public async Task CloseJobAsync(
    int recruiterId,
    int jobId)
        {
            var job = await _jobRepository.GetByIdAsync(
                jobId,
                trackChanges: true);

            if (job is null)
            {
                throw new KeyNotFoundException(
                    "Job was not found.");
            }

            if (job.RecruiterId != recruiterId)
            {
                throw new UnauthorizedAccessException(
                    "You are not allowed to close this job.");
            }

            if (job.IsClosed)
            {
                throw new InvalidOperationException(
                    "This job is already closed.");
            }

            job.IsClosed = true;
            job.UpdatedAt = DateTime.UtcNow;

            await _jobRepository.SaveChangesAsync();
        }
    }
}
