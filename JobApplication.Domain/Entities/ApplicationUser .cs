using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplication.Domain.Entities
{
        public class ApplicationUser : IdentityUser<int>
        {
        public string FirstName { get; set; } = default!;

        public string LastName { get; set; } = default!;
        public string? CvUrl { get; set; }
        }
    }
