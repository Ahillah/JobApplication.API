using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplication.Application.DTO_s.ApplyJobDto
{
    public class JobApplicationListDto
    {

        public int Id { get; set; }
        public int CandidateId { get; set; }
        public string CandidateName { get; set; } = default!;
        public string JobApplicationStatus { get; set; } = default!;
        public string CvUrl { get; set; } = default!;
        public DateTime AppliedAt { get; set; }
    }
}
