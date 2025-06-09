using HotChocolate.Subscriptions;
using LaundryCleaning.Common.Constants;
using LaundryCleaning.Common.Models.Entities;
using LaundryCleaning.GraphQL.Users.CustomModels;
using LaundryCleaning.GraphQL.Users.Inputs;
using LaundryCleaning.GraphQL.Users.Services.Interfaces;

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
    }
}
