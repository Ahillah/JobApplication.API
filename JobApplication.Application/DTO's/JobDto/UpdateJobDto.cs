using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplication.Application.DTO_s.JobDto
{
    public class UpdateJobDto
    {
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
    }
}
