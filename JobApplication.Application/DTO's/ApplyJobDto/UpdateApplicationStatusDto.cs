using JobApplication.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplication.Application.DTO_s.ApplyJobDto
{
    public class UpdateApplicationStatusDto
    {
        public JobApplicationStatus Status { get; set; }
    }
}

