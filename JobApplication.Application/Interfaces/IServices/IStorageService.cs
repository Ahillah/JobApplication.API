using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplication.Application.Interfaces.IServices
{
    public interface IStorageService
    {
        Task<string> UploadFileAsync(IFormFile file);
        Task DeleteAsync(string path);

    }
}
