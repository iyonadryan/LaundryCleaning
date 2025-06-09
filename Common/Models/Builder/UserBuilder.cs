using LaundryCleaning.Common.Models.Entities;
using LaundryCleaning.Data;
using LaundryCleaning.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LaundryCleaning.Common.Models.Builder
{
    public class UserBuilder
    {
        private readonly ApplicationDbContext _dbContext;

        public UserBuilder(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.
                ApplyEntityDefaults(_dbContext);

            builder
                .Property(x => x.Email)
                .HasMaxLength(50);

            builder
                .Property(x => x.Username)
                .HasMaxLength(30);

            builder
                .Property(x => x.FirstName)
                .HasMaxLength(20);

            builder
                .Property(x => x.LastName)
                .HasMaxLength(20);
        }
    }
}
