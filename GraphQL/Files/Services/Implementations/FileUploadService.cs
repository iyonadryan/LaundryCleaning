using LaundryCleaning.Common.Inputs;
using LaundryCleaning.Common.Response;
using LaundryCleaning.GraphQL.Files.Services.Interfaces;
using LaundryCleaning.GraphQL.Users.Services.Implementations;
using Microsoft.AspNetCore.Http;

namespace LaundryCleaning.GraphQL.Files.Services.Implementations
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<FileUploadService> _logger;
        public FileUploadService(
            IHttpContextAccessor httpContextAccessor
            ,ILogger<FileUploadService> logger) 
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<GlobalUploadFileResponseCustomModel> UploadFile(GlobalUploadFileInput input, CancellationToken cancellationToken)
        {
            var file = input.File;

            var ext = System.IO.Path.GetExtension(file.Name);
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var random = GenerateRandomString(10);
            var newFileName = $"{timestamp}_{random}{ext}";

            var filePath = System.IO.Path.Combine("wwwroot","Uploads", newFileName);

            await using var stream = File.Create(filePath);
            await file.CopyToAsync(stream, cancellationToken);

            var request = _httpContextAccessor.HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";
            var fileUrl = $"{baseUrl}/Uploads/{newFileName}";

            return new GlobalUploadFileResponseCustomModel() { 
                Success = true,
                Url = fileUrl
            };
        }

        private static string GenerateRandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
