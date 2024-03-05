using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AMaz.Entity; 

namespace AMaz.DB.DbContexts
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasData(
                new User
                {
                    UserId = Guid.NewGuid(),
                    FirstName = "Minh Huy",
                    LastName = "Huynh",
                    Email = "huy@gmail.com",
                    Password = "123"
                }
            );
        }
    }
}
