using LaundryCleaning.Common.Models.Entities;
using LaundryCleaning.Data;
using LaundryCleaning.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LaundryCleaning.Common.Models.Builder
{
    public class RoleBuilder
    {
        private readonly ApplicationDbContext _dbContext;

        public RoleBuilder(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.
                ApplyEntityDefaults(_dbContext);

            builder
                .Property(x => x.Name)
                .HasMaxLength(20);

            builder
                .Property(x => x.Code)
                .HasMaxLength(20);
        }
    }
}
