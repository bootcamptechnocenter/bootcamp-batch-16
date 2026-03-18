using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Shared.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IDMS.Infrastructure.Configurations
{
    public class MstUserConfiguration : IEntityTypeConfiguration<MstUser>
    {
        public void Configure(EntityTypeBuilder<MstUser> builder)
        {
            builder.ToTable("mst_user");

            builder.HasKey(e => e.Id)
                .HasName("pk_mst_user");

            builder.Property(e => e.Id)
                .HasColumnName("id")
                .UseSerialColumn();

            builder.Property(e => e.Email)
                .HasColumnName("email")
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.Password)
                .HasColumnName("password")
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(e => e.FullName)
                .HasColumnName("full_name")
                .IsRequired()
                .HasMaxLength(100);


            builder.Property(e => e.CreatedAt)
                .HasColumnName("created_at") // Ini yang bikin error kalau tidak ada
                .HasColumnType("timestamp");

            builder.Property(e => e.CreatedBy)
                .HasColumnName("created_by")
                .HasMaxLength(100);

            builder.Property(e => e.UpdatedAt)
                .HasColumnName("updated_at")
                .HasColumnType("timestamp");

            builder.Property(e => e.UpdatedBy)
                .HasColumnName("updated_by")
                .HasMaxLength(100);

            builder.Property(e => e.DeletedAt)
                .HasColumnName("deleted_at")
                .HasColumnType("timestamp");

            builder.Property(e => e.DeletedBy)
                .HasColumnName("deleted_by")
                .HasMaxLength(100);

        }
    }
}