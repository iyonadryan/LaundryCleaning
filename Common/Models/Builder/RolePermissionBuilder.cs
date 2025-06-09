using LaundryCleaning.Common.Models.Entities;
using LaundryCleaning.Data;
using LaundryCleaning.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LaundryCleaning.Common.Models.Builder
{
    public class RolePermissionBuilder
    {
        private readonly ApplicationDbContext _dbContext;

        public RolePermissionBuilder(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder
                .Property(x => x.Permission)
                .HasMaxLength(50);
        }
    }
}
