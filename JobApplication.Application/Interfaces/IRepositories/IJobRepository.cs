using JobApplication.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplication.Application.Interfaces.IRepositories
{
    public interface IJobRepository : IGenericRepository<Job, int>
    {
    }
}
