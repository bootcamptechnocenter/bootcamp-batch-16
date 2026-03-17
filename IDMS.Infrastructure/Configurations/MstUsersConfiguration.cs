using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Shared.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IDMS.Infrastructure.Configurations
{
    public class MstUsersConfiguration : IEntityTypeConfiguration<MstUsers>
    {
        public void Configure(EntityTypeBuilder<MstUsers> builder)
        {
            builder.ToTable("mst_users");

            builder.HasKey(e => e.Id)
                .HasName("pk_mst_users");

            builder.Property(e => e.Id)
                .HasColumnName("id")
                .UseSerialColumn();

            builder.Property(e => e.Email)
                .HasColumnName("email")
                .HasMaxLength(255)
                .IsRequired()
                .IsUnicode(false);

            builder.Property(e => e.FullName)
                .HasColumnName("full_name")
                .HasMaxLength(255)
                .IsRequired()
                .IsUnicode(false);

            builder.Property(e => e.Password)
                .HasColumnName("password")
                .HasMaxLength(255)
                .IsRequired()
                .IsUnicode(false);

            builder.Property(e => e.UserName)
                .HasColumnName("user_name")
                .HasMaxLength(255)
                .IsRequired()
                .IsUnicode(false);

            builder.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamp")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

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