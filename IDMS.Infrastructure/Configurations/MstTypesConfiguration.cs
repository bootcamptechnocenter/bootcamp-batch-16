using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Shared.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace IDMS.Infrastructure.Configurations
{
    public class MstTypesConfiguration : IEntityTypeConfiguration<MstTypes>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<MstTypes> builder)
        {
            builder.ToTable("mst_types");

            builder.HasKey(e => e.Id)
                .HasName("mst_types_pkey");

            builder.Property(e => e.Id)
                .HasColumnName("id")
                .UseSerialColumn();

            builder.Property(e => e.BrandId)
                .IsRequired()
                .HasColumnType("integer")
                .HasColumnName("brand_id");

            builder.Property(e => e.Code)
                .IsRequired()
                .HasMaxLength(10)
                .HasColumnType("varchar(10)")
                .HasColumnName("code");

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("varchar(100)")
                .HasColumnName("name");

            builder.Property(e => e.IsActive)
                .IsRequired()
                .HasColumnName("is_active");

            builder.Property(e => e.CreatedBy)
                .HasColumnName("created_by")
                .HasMaxLength(100)
                .HasColumnType("varchar(100)");

            builder.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamp");

            builder.Property(e => e.UpdatedBy)
                .HasColumnName("updated_by")
                .HasMaxLength(100)
                .HasColumnType("varchar(100)");

            builder.Property(e => e.UpdatedAt)
                .HasColumnName("updated_at")
                .HasColumnType("timestamp"); 

            builder.Property(e => e.DeletedBy)
                .HasColumnName("deleted_by")
                .HasMaxLength(100)
                .HasColumnType("varchar(100)");

            builder.Property(e => e.DeletedAt)
                .HasColumnName("deleted_at")
                .HasColumnType("timestamp");

            builder.HasOne(e => e.Brand)
                .WithMany()
                .HasForeignKey(e => e.BrandId)
                .HasConstraintName("mst_types_brand_id_fkey");;
        }
    }
}