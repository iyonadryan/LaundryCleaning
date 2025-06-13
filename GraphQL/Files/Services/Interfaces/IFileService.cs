using LaundryCleaning.Common.Inputs;
using LaundryCleaning.Common.Response;

namespace LaundryCleaning.GraphQL.Files.Services.Interfaces
{
    public interface IFileService
    {
        Task<GlobalUploadFileResponseCustomModel> UploadFile(GlobalUploadFileInput input, CancellationToken cancellationToken);
        Task<GlobalUploadFileResponseCustomModel> GenerateExcelFile(CancellationToken cancellationToken);
        Task<List<List<string>>> UploadExcelAndReadRows(GlobalUploadFileInput input, CancellationToken cancellationToken);
        Task<GlobalUploadFileResponseCustomModel> GenerateInvoice(CancellationToken cancellationToken);
    }
}
