using LaundryCleaning.Common.Response;
using LaundryCleaning.GraphQL.Roles.Inputs;

namespace LaundryCleaning.GraphQL.Roles.Services.Interfaces
{
    public interface IRoleService
    {
        Task<GlobalSuccessResponseCustomModel> AddRolePermission(AddRolePermissionInput input, CancellationToken cancellationToken);
    }
}
