using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplication.Domain.Entities
{
    public class Job: BaseEntity<int>
    {
        public int RecruiterId { get; set; }

        public ApplicationUser Recruiter { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public ICollection<JobCandidateApplication> Applications { get; set; }
       = new List<JobCandidateApplication>();
    }

}

