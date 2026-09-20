using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplication.Application.DTO_s.JobDto
{
    public class JobDetailsDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public bool IsActive { get; set; }
        public bool IsClosed { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
