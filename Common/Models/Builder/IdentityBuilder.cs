using LaundryCleaning.Common.Models.Entities;
using LaundryCleaning.Data;
using LaundryCleaning.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LaundryCleaning.Common.Models.Builder
{
    public class IdentityBuilder
    {
        private readonly ApplicationDbContext _dbContext;

        public IdentityBuilder(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Configure(EntityTypeBuilder<Identity> builder)
        {
            builder.
                ApplyEntityDefaults(_dbContext);
        }
    }
}
