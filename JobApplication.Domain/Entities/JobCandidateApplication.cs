using JobApplication.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplication.Domain.Entities
{
    public class JobCandidateApplication : BaseEntity<int>
    {

        public int ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public int JobId { get; set; }
       
        public Job Job { get; set; }
        public JobApplicationStatus JobApplicationStatus { get; set; }
      
    }
}
