using LaundryCleaning.Common.Models.Entities;
using LaundryCleaning.GraphQL.Users.CustomModels;
using LaundryCleaning.GraphQL.Users.Inputs;

namespace LaundryCleaning.GraphQL.Users.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> GetUsers (CancellationToken cancellationToken);
        Task<CreateUserCustomModel> CreateUser(CreateUserInput input, CancellationToken cancellationToken);
    }
}
