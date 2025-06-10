using LaundryCleaning.Common.Inputs;
using LaundryCleaning.Common.Response;

namespace LaundryCleaning.GraphQL.Files.Services.Interfaces
{
    public interface IFileUploadService
    {
        Task<GlobalUploadFileResponseCustomModel> UploadFile(GlobalUploadFileInput input, CancellationToken cancellationToken);
    }
}
