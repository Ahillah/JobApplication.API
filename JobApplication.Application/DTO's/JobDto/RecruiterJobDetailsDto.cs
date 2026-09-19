using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplication.Application.DTO_s.JobDto
{
    public class RecruiterJobDetailsDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ApplicationsCount { get; set; }
    }
}
