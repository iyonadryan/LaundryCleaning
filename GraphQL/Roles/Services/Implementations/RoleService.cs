using HotChocolate.Subscriptions;
using LaundryCleaning.Common.Exceptions;
using LaundryCleaning.Common.Models.Entities;
using LaundryCleaning.Common.Response;
using LaundryCleaning.Data;
using LaundryCleaning.GraphQL.Roles.Inputs;
using LaundryCleaning.GraphQL.Roles.Services.Interfaces;
using LaundryCleaning.Security.Permissions;
using LaundryCleaning.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace LaundryCleaning.GraphQL.Roles.Services.Implementations
{
    public class RoleService : IRoleService
    {
        private readonly ApplicationDbContext _dbContext;

        public RoleService(
            ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GlobalSuccessResponseCustomModel> AddRolePermission(AddRolePermissionInput input,CancellationToken cancellationToken)
        {
            var response = new GlobalSuccessResponseCustomModel();

            var roleExist = await _dbContext.Roles.Where(x => x.Id.Equals(input.RoleId)).FirstOrDefaultAsync(cancellationToken);

            if (roleExist == null)
            {
                throw new BusinessLogicException("Role not found!.");
            }

            var permissionExist = PermissionConstants.AllPermission.Any(p => input.Permission.Contains(p));
            if (permissionExist == false)
            {
                throw new BusinessLogicException("Permission not found!.");
            }

            var rolePermissionExist = await _dbContext.RolePermissions
                .Where(x => x.RoleId.Equals(input.RoleId)
                    && x.Permission == input.Permission)
                .FirstOrDefaultAsync(cancellationToken);

            if (rolePermissionExist != null)
            {
                throw new BusinessLogicException("Role Permission already exist!.");
            }

            var newRolePermission = new RolePermission()
            {
                RoleId = input.RoleId,
                Permission = input.Permission
            };

            await _dbContext.RolePermissions.AddAsync(newRolePermission, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            response = new GlobalSuccessResponseCustomModel()
            {
                Success = true,
                Message = "Role Permission has been added"
            };
            return response;


        }
    }
}
