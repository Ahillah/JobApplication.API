using JobApplication.Application.Interfaces.IRepositories;
using JobApplication.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplication.Infrastructure.Repositories
{
    public class JobRepository
        : GenericRepository<Job, int>, IJobRepository
    {
        public JobRepository(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}

