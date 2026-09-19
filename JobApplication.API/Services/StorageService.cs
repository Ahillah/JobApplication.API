using JobApplication.Application.Interfaces.IServices;

namespace JobApplication.API.Services
{
    public class StorageService:IStorageService
    {
        private readonly IWebHostEnvironment _environment;
        private const long MaxFileSizeBytes = 50 * 1024 * 1024;
        private readonly string[] _allowedFileExtensions =
     {
            ".pdf",
            ".doc",
            ".docx",
            ".ppt",
            ".pptx",
            ".xls",
            ".xlsx"
        };
        public StorageService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public Task<string> UploadFileAsync(IFormFile file)
        {
            return UploadAsync(file, "files", _allowedFileExtensions);
        }
        private async Task<string> UploadAsync(
     IFormFile file,
     string folderName,
     string[] allowedExtensions)
        {
            if (file is null || file.Length == 0)
                throw new Exception("الملف مطلوب ولا يمكن أن يكون فارغاً.");

            if (file.Length > MaxFileSizeBytes)
                throw new Exception($"حجم الملف يتجاوز الحد الأقصى المسموح به وهو ({MaxFileSizeBytes / (1024 * 1024)} ميجابايت).");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
                throw new Exception("صيغة أو امتداد الملف غير مدعوم.");

            var fileName = $"{Guid.NewGuid()}{extension}";

            var folderPath = Path.Combine(
                _environment.WebRootPath,
                "uploads",
                folderName);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var filePath = Path.Combine(folderPath, fileName);

            using var fs = new FileStream(filePath, FileMode.Create);

            await file.CopyToAsync(fs);

            return $"/uploads/{folderName}/{fileName}";
        }
        public Task DeleteAsync(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return Task.CompletedTask;

            var fullPath = Path.Combine(
                _environment.WebRootPath,
                path.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

            return Task.CompletedTask;
        }
    }
}
