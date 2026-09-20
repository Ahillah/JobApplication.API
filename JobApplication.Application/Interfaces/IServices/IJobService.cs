using JobApplication.Application.DTO_s.JobDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplication.Application.Interfaces.IServices
{
    public interface IJobService
    {
        Task CreateJobAsync(
           int recruiterId,
           CreateJobDto dto);

        Task UpdateJobAsync(
           int recruiterId,
           int jobId,
           UpdateJobDto dto);

        Task UpdateJobStatusAsync(
            int recruiterId,
            int jobId,
            UpdateJobStatusDto dto);

        Task<JobDetailsDto> GetJobDetailsAsync(
            int jobId);

        Task CloseJobAsync(
    int recruiterId,
    int jobId);
    }
}
