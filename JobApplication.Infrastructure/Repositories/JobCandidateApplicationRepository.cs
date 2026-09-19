using JobApplication.Application.Interfaces.IRepositories;
using JobApplication.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplication.Infrastructure.Repositories
{
    public class JobCandidateApplicationRepository
       : GenericRepository<JobCandidateApplication, int>,
         IJobCandidateApplicationRepository
    {
        public JobCandidateApplicationRepository(
            ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task<bool> ExistsForCandidateAsync(
          int candidateId,
          int jobId)
        {
            return await _dbSet
                .AsNoTracking()
                .AnyAsync(x =>
                    x.ApplicationUserId == candidateId &&
                    x.JobId == jobId);
        }
        public async Task<IReadOnlyList<JobCandidateApplication>>
    GetByCandidateIdAsync(int candidateId)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(x => x.Job)
                .Where(x => x.ApplicationUserId == candidateId)
                .ToListAsync();
        }

    }
}
