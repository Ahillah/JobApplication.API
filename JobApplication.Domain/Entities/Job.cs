using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplication.Domain.Entities
{
    public class Job: BaseEntity<int>
    {
       
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public ICollection<JobCandidateApplication> Applications { get; set; }
       = new List<JobCandidateApplication>();
    }

}

