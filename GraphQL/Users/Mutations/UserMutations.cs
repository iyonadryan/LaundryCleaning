using HotChocolate.Subscriptions;
using LaundryCleaning.Common.Constants;
using LaundryCleaning.Common.Models.Entities;
using LaundryCleaning.GraphQL.Users.CustomModels;
using LaundryCleaning.GraphQL.Users.Inputs;
using LaundryCleaning.GraphQL.Users.Services.Interfaces;
using LaundryCleaning.Security;
using LaundryCleaning.Security.Permissions;

namespace LaundryCleaning.GraphQL.Users.Mutations
{
    [ExtendObjectType(ExtendObjectTypeConstants.Mutation)]
    public class UserMutations
    {
        [GraphQLName("createUser")]
        [GraphQLDescription("Create new user.")]
        public async Task<CreateUserCustomModel> CreateUser(
            CreateUserInput input,
            [Service] IUserService service,
            CancellationToken cancellationtoken)
        {
            return await service.CreateUser(input, cancellationtoken);
        }

        [RequirePermission(PermissionConstants.UserManage)]
        [GraphQLName("sendUserNotification")]
        [GraphQLDescription("Send User Notification.")]
        public async Task<string> SendUserNotification(
            string input,
            [Service] IUserService service,
            CancellationToken cancellationtoken)
        {
            return await service.SendUserNotification(input, cancellationtoken);
        }
    }
}
