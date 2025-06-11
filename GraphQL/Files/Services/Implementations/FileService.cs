using LaundryCleaning.Common.Inputs;
using LaundryCleaning.Common.Response;
using LaundryCleaning.Data;
using LaundryCleaning.Download;
using LaundryCleaning.GraphQL.Files.Services.Interfaces;
using LaundryCleaning.GraphQL.Users.Services.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NPOI.HPSF;
using NPOI.XSSF.UserModel;

namespace LaundryCleaning.GraphQL.Files.Services.Implementations
{
    public class FileService : IFileService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<FileService> _logger;
        private readonly SecureDownloadHelper _secureDownloadHelper;
        public FileService(
            ApplicationDbContext dbContext
            ,IHttpContextAccessor httpContextAccessor
            ,ILogger<FileService> logger
            ,SecureDownloadHelper secureDownloadHelper) 
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _secureDownloadHelper = secureDownloadHelper;
        }

        public async Task<GlobalUploadFileResponseCustomModel> UploadFile(GlobalUploadFileInput input, CancellationToken cancellationToken)
        {
            var file = input.File;

            var ext = System.IO.Path.GetExtension(file.Name);
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var random = GenerateRandomString(10);
            var newFileName = $"{timestamp}_{random}{ext}";

            var filePath = System.IO.Path.Combine("wwwroot","Uploads", newFileName);

            // Buat folder jika belum ada
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(filePath)!);

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

        public async Task<GlobalUploadFileResponseCustomModel> GenerateExcelFile(CancellationToken cancellationToken) 
        {
            // Create new workbook
            var workbook = new XSSFWorkbook();
            var sheet = workbook.CreateSheet("Sheet1");

            // Add header
            var headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("ID");
            headerRow.CreateCell(1).SetCellValue("Name");
            headerRow.CreateCell(2).SetCellValue("Email");

            // Add data
            var data = await (from u in _dbContext.Users
                              select u).ToListAsync(cancellationToken);

            int startIndex = 1;
            foreach (var user in data)
            {
                var row = sheet.CreateRow(startIndex);
                row.CreateCell(0).SetCellValue(startIndex);
                row.CreateCell(1).SetCellValue(user.Username);
                row.CreateCell(2).SetCellValue(user.Email);
                startIndex++;
            }

            // Generate file name
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var random = GenerateRandomString(10);
            var newFileName = $"{timestamp}_{random}.xlsx";

            var filePath = System.IO.Path.Combine("Storages", "temp", newFileName);

            // Buat folder jika belum ada
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(filePath)!);

            using var ms = new MemoryStream();
            workbook.Write(ms);
            await File.WriteAllBytesAsync(filePath, ms.ToArray(),cancellationToken);

            var token = _secureDownloadHelper.GenerateToken(newFileName, TimeSpan.FromMinutes(5));

            var request = _httpContextAccessor.HttpContext!.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";
            var url = $"{baseUrl}/download?token={Uri.EscapeDataString(token)}";

            return new GlobalUploadFileResponseCustomModel()
            {
                Success = true,
                Url = url
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
