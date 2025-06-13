using LaundryCleaning.Common.Constants;
using LaundryCleaning.Common.Inputs;
using LaundryCleaning.Common.Response;
using LaundryCleaning.GraphQL.Files.Services.Interfaces;
using LaundryCleaning.Security;
using LaundryCleaning.Security.Permissions;

namespace LaundryCleaning.GraphQL.Files.Mutations
{
    [ExtendObjectType(ExtendObjectTypeConstants.Mutation)]
    public class FileMutations
    {
        [RequirePermission(PermissionConstants.FileUpload)]
        [GraphQLName("uploadFile")]
        [GraphQLDescription("Upload File.")]
        public async Task<GlobalUploadFileResponseCustomModel> UploadFile(
            GlobalUploadFileInput input,
            [Service] IFileService service,
            CancellationToken cancellationToken)
        {
            return await service.UploadFile(input, cancellationToken);

        }

        [RequirePermission(PermissionConstants.FileUpload)]
        [GraphQLName("generateExcelFile")]
        [GraphQLDescription("Generate Excel File.")]
        public async Task<GlobalUploadFileResponseCustomModel> GenerateExcelFile(
            [Service] IFileService service,
            CancellationToken cancellationToken)
        {
            return await service.GenerateExcelFile(cancellationToken);

        }

        [RequirePermission(PermissionConstants.FileUpload)]
        [GraphQLName("uploadExcelAndReadRows")]
        [GraphQLDescription("Upload Excel And Read Rows.")]
        public async Task<List<List<string>>> UploadExcelAndReadRows(
            GlobalUploadFileInput input,
            [Service] IFileService service,
            CancellationToken cancellationToken)
        {
            return await service.UploadExcelAndReadRows(input, cancellationToken);
        }

        [RequirePermission(PermissionConstants.FileUpload)]
        [GraphQLName("generateInvoice")]
        [GraphQLDescription("Generate Invoice.")]
        public async Task<GlobalUploadFileResponseCustomModel> GenerateInvoice(
            [Service] IFileService service,
            CancellationToken cancellationToken)
        {
            return await service.GenerateInvoice(cancellationToken);

        }
    }
}
