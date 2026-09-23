using JobApplication.Application.Interfaces.IRepositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplication.Application.Features.CloseJob.Commands.UpdateJobToBeClosed
{
    public class CloseJobHandler : IRequestHandler<CloseJobCommand>
    {
        private readonly IJobRepository _jobRepository;

        public  CloseJobHandler(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }
        public async Task Handle(CloseJobCommand request, CancellationToken cancellationToken)
        {
            var job = await _jobRepository.GetByIdAsync(
               request.JobId,
               trackChanges: true);

            if (job is null)
            {
                throw new KeyNotFoundException(
                    "Job was not found.");
            }

            if (job.RecruiterId != request.RecruiterId)
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
