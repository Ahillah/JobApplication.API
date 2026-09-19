using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplication.Domain.Enums
{
    public enum JobApplicationStatus
    {
        Applied,
        UnderReview,
        InterView,
        Accepted,
        Rejected,
        Cancelled
    }
}
