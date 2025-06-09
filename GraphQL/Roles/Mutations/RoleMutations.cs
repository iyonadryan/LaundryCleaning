using LaundryCleaning.Common.Constants;
using LaundryCleaning.Common.Response;
using LaundryCleaning.GraphQL.Roles.Inputs;
using LaundryCleaning.GraphQL.Roles.Services.Interfaces;
using LaundryCleaning.Security;
using LaundryCleaning.Security.Permissions;

namespace LaundryCleaning.GraphQL.Roles.Mutations
{
    [ExtendObjectType(ExtendObjectTypeConstants.Mutation)]
    public class RoleMutations
    {
        [RequirePermission(PermissionConstants.RoleManage)]
        [GraphQLName("addRolePermission")]
        [GraphQLDescription("Add Role Permission.")]
        public async Task<GlobalSuccessResponseCustomModel> AddRolePermission(
            AddRolePermissionInput input,
            [Service] IRoleService service,
            CancellationToken cancellationToken)
        {
            return await service.AddRolePermission(input, cancellationToken);

        }
    }
}
