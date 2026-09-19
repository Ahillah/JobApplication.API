using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplication.Application.DTO_s.ApplyJobDto
{
    public class ApplyForJobDto
    {
        public int JobId { get; set; }

        public IFormFile Cv { get; set; } = default!;
    }
}
