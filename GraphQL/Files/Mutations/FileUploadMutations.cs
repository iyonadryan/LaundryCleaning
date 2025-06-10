using LaundryCleaning.Common.Constants;
using LaundryCleaning.Common.Inputs;
using LaundryCleaning.Common.Response;
using LaundryCleaning.GraphQL.Files.Services.Interfaces;
using LaundryCleaning.Security;
using LaundryCleaning.Security.Permissions;

namespace LaundryCleaning.GraphQL.Files.Mutations
{
    [ExtendObjectType(ExtendObjectTypeConstants.Mutation)]
    public class FileUploadMutations
    {
        [RequirePermission(PermissionConstants.FileUpload)]
        [GraphQLName("uploadFile")]
        [GraphQLDescription("Upload File.")]
        public async Task<GlobalUploadFileResponseCustomModel> UploadFile(
            GlobalUploadFileInput input,
            [Service] IFileUploadService service,
            CancellationToken cancellationToken)
        {
            return await service.UploadFile(input, cancellationToken);

        }
    }
}
