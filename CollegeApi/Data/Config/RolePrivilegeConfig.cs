using CollegeApi.Data.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollegeApi.Data.Config
{
    public class RolePrivilegeConfig : IEntityTypeConfiguration<RolePrivilege>
    {
        public void Configure(EntityTypeBuilder<RolePrivilege> builder)
        {
            builder.ToTable("RolePrivileges");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.RolePrivilegeName).IsRequired().HasMaxLength(250);
            builder.Property(x => x.Description);
            builder.Property(x => x.IsActive).IsRequired();
            builder.Property(x => x.IsDeleted).IsRequired();
            builder.Property(x => x.CreatedDate).IsRequired();

            builder.HasOne(n => n.Role)
                .WithMany(n => n.RolePrivileges)
                .HasForeignKey(n => n.RoleId)
                .HasConstraintName("FK_RolePrivileges_Roles");
        }
    }
}
