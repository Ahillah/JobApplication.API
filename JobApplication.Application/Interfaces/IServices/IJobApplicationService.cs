using JobApplication.Application.DTO_s.ApplyJobDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplication.Application.Interfaces.IServices
{
    public interface IJobApplicationService
    {
        Task ApplyForJobAsync(
            int candidateId,
            ApplyForJobDto dto);
        Task<IReadOnlyList<CandidateApplicationDto>>
        GetMyApplicationsAsync(int candidateId);

        Task CancelApplicationAsync(
    int candidateId,
    int applicationId);


        Task<IReadOnlyList<JobApplicationDetailsDto>>
        GetJobApplicationsAsync(
            int recruiterId,
            int jobId);
        Task UpdateApplicationStatusAsync(
    int recruiterId,
    int applicationId,
    UpdateApplicationStatusDto dto);

    }
}