using LaundryCleaning.Common.Constants;
using LaundryCleaning.Common.Models.Entities;
using LaundryCleaning.GraphQL.Users.CustomModels;
using LaundryCleaning.GraphQL.Users.Inputs;
using LaundryCleaning.GraphQL.Users.Services.Interfaces;
using LaundryCleaning.Security;
using LaundryCleaning.Security.Permissions;
using Microsoft.EntityFrameworkCore;

namespace LaundryCleaning.GraphQL.Users.Queries
{
    [ExtendObjectType(ExtendObjectTypeConstants.Query)]
    public class UserQueries
    {
        [RequirePermission(PermissionConstants.UserManage)]
        [GraphQLName("getUsers")]
        [GraphQLDescription("Get all user.")]
        public async Task<List<User>> GetUsers(
            [Service] IUserService service,
            CancellationToken cancellationToken)
        {
            return await service.GetUsers(cancellationToken);        
        }
    }
}
