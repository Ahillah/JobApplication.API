using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplication.Application.DTO_s.Identity
{
    public class AuthResponseDto
    {

        public bool IsSuccess { get; set; }

        public string? Token { get; set; }

        public string? Email { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Role { get; set; }

        public List<string> Errors { get; set; } = new();
    }
}