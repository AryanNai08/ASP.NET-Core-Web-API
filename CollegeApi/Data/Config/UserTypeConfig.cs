using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollegeApi.Data.Config
{
    public class UserTypeConfig : IEntityTypeConfiguration<UserType>
    {
        public void Configure(EntityTypeBuilder<UserType> builder)
        {
            builder.ToTable("UserTypes");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.Name).IsRequired().HasMaxLength(250);
            builder.Property(x => x.Description).HasMaxLength(1500);

            builder.HasData(new List<UserType>()
            {
                new UserType
                {
                    Id = 1,
                    Name = "Student",
                    Description = "For Student"
                },
                new UserType
                {
                    Id = 2,
                    Name = "Faculty",
                    Description = "For Faculty"
                },
                new UserType
                {
                    Id = 3,
                    Name = "Supporting Staff",
                    Description = "For Supporting Staff"
                },
                new UserType
                {
                    Id = 4,
                    Name = "Parents",
                    Description = "For Parents"
                }
            });


        }
    }
}
