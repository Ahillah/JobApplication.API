using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplication.Application.Features.CloseJob.Commands.UpdateJobToBeClosed
{
    public class CloseJobCommand : IRequest
    {
        public int RecruiterId { get; set; }
        public int JobId { get; set; }
    }
}
