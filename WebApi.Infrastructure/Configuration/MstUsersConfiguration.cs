using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Shared.Domain.Entities;

namespace WebApi.Infrastructure.Configuration
{
    public class MstUsersConfiguration : IEntityTypeConfiguration<MstUsers>
    {
        public void Configure(EntityTypeBuilder<MstUsers> builder)
        {
            builder.ToTable("mst_users");

            builder.Property(e => e.Id)
                .HasColumnName("id")
                .UseSerialColumn();

            builder.HasKey(e => e.Id)
                .HasName("mst_users_pkey");

            builder.Property(e => e.Email)
                .HasColumnName("email")
                .HasColumnType("varchar(50)")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(e => e.Password)
                .HasColumnName("password")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.FullName)
                .HasColumnName("full_name")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamp");

            builder.Property(e => e.CreatedBy)
                .HasColumnName("created_by")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            builder.Property(e => e.UpdatedAt)
                .HasColumnName("updated_at")
                .HasColumnType("timestamp");

            builder.Property(e => e.UpdatedBy)
                .HasColumnName("updated_by")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            builder.Property(e => e.DeletedAt)
                .HasColumnName("deleted_at")
                .HasColumnType("timestamp");

            builder.Property(e => e.DeletedBy)
               .HasColumnName("deleted_by")
               .HasColumnType("varchar(100)")
               .HasMaxLength(100);
        }
    }
}