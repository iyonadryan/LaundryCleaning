using HotChocolate.Subscriptions;
using LaundryCleaning.Common.Exceptions;
using LaundryCleaning.Common.Models.Entities;
using LaundryCleaning.Common.Models.Messages;
using LaundryCleaning.Data;
using LaundryCleaning.GraphQL.Users.CustomModels;
using LaundryCleaning.GraphQL.Users.Inputs;
using LaundryCleaning.GraphQL.Users.Services.Interfaces;
using LaundryCleaning.GraphQL.Users.Subscriptions;
using LaundryCleaning.Services.Implementations;
using LaundryCleaning.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;

namespace LaundryCleaning.GraphQL.Users.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IPasswordService _passwordService;
        private readonly ITopicEventSender _topicEventSender;
        private readonly ILogger<UserService> _logger;

        public UserService(
            ApplicationDbContext dbContext,
            IPasswordService passwordService,
            ITopicEventSender topicEventSender,
            ILogger<UserService> logger)
        {
            _dbContext = dbContext;
            _passwordService = passwordService;
            _topicEventSender = topicEventSender;
            _logger = logger;
        }

        public async Task<List<User>> GetUsers(CancellationToken cancellationToken)
        {
            var data = await (from u in _dbContext.Users
                              select u).ToListAsync(cancellationToken);

            return data;
        }

        public async Task<CreateUserCustomModel> CreateUser(CreateUserInput input, CancellationToken cancellationToken)
        {
            var emailExist = await _dbContext.Users.Where(x => x.Email.Equals(input.Email)).FirstOrDefaultAsync(cancellationToken);

            if (emailExist != null)
            {
                throw new BusinessLogicException("Email already used, please use another Email!.");
            }

            var newUser = new User() {
                Email = input.Email,
                Password = _passwordService.GeneratePassword(input.Password),
                Username = input.Username?? string.Empty,
                FirstName = input.FirstName ?? string.Empty,
                LastName = input.LastName ?? string.Empty
            };

            await _dbContext.Users.AddAsync(newUser, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("New User has been created");

            // send subscription
            await _topicEventSender.SendAsync(nameof(UserSubscriptions.OnUserCreated), new UserCreated($"New User {newUser.Email} has been created" ), cancellationToken);

            var response = new CreateUserCustomModel() { 
                Success = true,
                Data = new UserCreatedResponse() {
                    Email = newUser.Email,
                    Username = newUser.Username,
                    FirstName = newUser.FirstName,
                    LastName = newUser.LastName,
                }
            };
            return response;
        }
    }
}
