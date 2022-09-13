using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using dotnet_2.Infrastructure.Data.Models;

namespace dotnet_2.Infrastructure.Data.Config
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(b => b.id)
            .HasColumnName("id");
        }
    }

    public class AuthTokennConfiguration : IEntityTypeConfiguration<AuthTokenn>
    {
        public void Configure(EntityTypeBuilder<AuthTokenn> builder)
        {
            builder.Property(b => b.id)
            .HasColumnName("id");
        }
    }
    public class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
    {
        public void Configure(EntityTypeBuilder<Organization> builder)
        {
            builder.Property(b => b.id)
            .HasColumnName("id");
        }
    }
    public class OvertimeConfiguration : IEntityTypeConfiguration<Overtime>
    {
        public void Configure(EntityTypeBuilder<Overtime> builder)
        {
            builder.Property(b => b.id)
            .HasColumnName("id");
        }
    }
}