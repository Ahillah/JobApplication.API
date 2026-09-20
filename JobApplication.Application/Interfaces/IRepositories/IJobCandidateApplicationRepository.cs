using JobApplication.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplication.Application.Interfaces.IRepositories
{
    public interface IJobCandidateApplicationRepository
         : IGenericRepository<JobCandidateApplication, int>
    {
        Task<bool> ExistsForCandidateAsync(
            int candidateId,
            int jobId);


        Task<IReadOnlyList<JobCandidateApplication>>
         GetByCandidateIdAsync(int candidateId);

        Task<IReadOnlyList<JobCandidateApplication>>
    GetByJobIdAsync(int jobId);
    }
}

