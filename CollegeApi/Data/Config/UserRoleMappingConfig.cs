using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollegeApi.Data.Config
{
    public class UserRoleMappingConfig : IEntityTypeConfiguration<UserRoleMapping>
    {
        public void Configure(EntityTypeBuilder<UserRoleMapping> builder)
        {
            builder.ToTable("UserRoleMappings");
            builder.HasKey(t => t.Id);

            builder.Property(x => x.Id).UseIdentityColumn();

            builder.HasIndex(n => new {n.UserId,n.RoleId},"UK_UserRoleMapping").IsUnique();

            builder.Property(n => n.UserId).IsRequired();
            builder.Property(n => n.RoleId).IsRequired();

            builder.HasOne(n => n.Role)
                .WithMany(n => n.UserRoleMappings)
                .HasForeignKey(n => n.RoleId)
                .HasConstraintName("FK_UserRoleMapping_Role");

            builder.HasOne(n => n.User)
                .WithMany(n => n.UserRoleMappings)
                .HasForeignKey(n => n.UserId)
                .HasConstraintName("FK_UserRoleMapping_User");

        }
    }
}
