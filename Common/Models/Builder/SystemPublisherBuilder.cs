using LaundryCleaning.Common.Models.Entities;
using LaundryCleaning.Data;
using LaundryCleaning.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LaundryCleaning.Common.Models.Builder
{
    public class SystemPublisherBuilder
    {
        private readonly ApplicationDbContext _dbContext;

        public SystemPublisherBuilder(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Configure(EntityTypeBuilder<SystemPublisher> builder)
        {
            builder.
                ApplyEntityDefaults(_dbContext);

            builder
                .Property(x => x.Topic)
                .HasMaxLength(50);
        }
    }
}
