using LaundryCleaning.Common.Models.Entities;
using LaundryCleaning.Data;
using LaundryCleaning.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LaundryCleaning.Common.Models.Builder
{
    public class InvoiceNumberTrackerBuilder
    {
        private readonly ApplicationDbContext _dbContext;

        public InvoiceNumberTrackerBuilder(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Configure(EntityTypeBuilder<InvoiceNumberTracker> builder)
        {
            builder.
                ApplyEntityDefaults(_dbContext);

            builder
                .Property(x => x.Code)
                .HasMaxLength(20);
        }
    }
}
