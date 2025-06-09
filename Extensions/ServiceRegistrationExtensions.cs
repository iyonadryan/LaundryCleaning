using HotChocolate.Subscriptions;
using LaundryCleaning.Auth.Services.Implementations;
using LaundryCleaning.Auth.Services.Interfaces;
using LaundryCleaning.GraphQL.Roles.Services.Implementations;
using LaundryCleaning.GraphQL.Roles.Services.Interfaces;
using LaundryCleaning.GraphQL.Users.Services.Implementations;
using LaundryCleaning.GraphQL.Users.Services.Interfaces;
using LaundryCleaning.Services.Implementations;
using LaundryCleaning.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace LaundryCleaning.Extensions
{
    public static class ServiceRegistrationExtensions
    {
        public static IServiceCollection AddProjectServices(this IServiceCollection services)
        {
            services.AddScoped<IPasswordHasher<object>, PasswordHasher<object>>();
            services.AddScoped<IPasswordService, PasswordService>();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
