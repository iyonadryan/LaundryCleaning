using LaundryCleaning.Common.Models.Entities;
using LaundryCleaning.Data;
using LaundryCleaning.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LaundryCleaning.Common.Models.Builder
{
    public class SystemReceivedBuilder
    {
        private readonly ApplicationDbContext _dbContext;

        public SystemReceivedBuilder(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Configure(EntityTypeBuilder<SystemReceived> builder)
        {
            builder.
                ApplyEntityDefaults(_dbContext);

            builder
                .Property(x => x.Topic)
                .HasMaxLength(50);
        }
    }
}
