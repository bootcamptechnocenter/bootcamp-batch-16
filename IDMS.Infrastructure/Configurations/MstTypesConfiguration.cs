using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Shared.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IDMS.Infrastructure.Configurations
{
    public class MstTypesConfiguration: IEntityTypeConfiguration<MstTypes>
    {
        public void Configure(EntityTypeBuilder<MstTypes> builder)
        {
            builder.ToTable("mst_types");
            builder.HasKey(e => e.Id)
            .HasName("mst_types_pkey");

            builder.Property(e => e.Id)
            .HasColumnName("id")
            .UseSerialColumn();

            builder.Property(e => e.Code)
            .HasColumnName("code")
            .HasColumnType("varchar(10)")
            .HasMaxLength(10);

            builder.Property(e => e.Name)
            .HasColumnName("name")
            .HasColumnType("varchar(100)")
            .HasMaxLength(100);

            builder.Property(e => e.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true);

            // brand_id
            builder.Property(e => e.MstBrandId)
            .HasColumnName("brand_id")
            .HasColumnType("int")
            .IsRequired();




            // created at
            builder.Property(e => e.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
            // created by
            builder.Property(e => e.CreatedBy)
            .HasColumnName("created_by")
            .HasColumnType("varchar(50)")
            .HasMaxLength(50);
            // updated at
            builder.Property(e => e.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("timestamp");
            // updated by
            builder.Property(e => e.UpdatedBy)
            .HasColumnName("updated_by")
            .HasColumnType("varchar(50)")
            .HasMaxLength(50);
            // deleted at
            builder.Property(e => e.DeletedAt)
            .HasColumnName("deleted_at")
            .HasColumnType("timestamp");
            // deleted by
            builder.Property(e => e.DeletedBy)
            .HasColumnName("deleted_by")
            .HasColumnType("varchar(50)")
            .HasMaxLength(50);

            // ralasi ke brand_id
            builder.HasOne(e => e.Brands)
            .WithMany()
            .HasForeignKey(e => e.MstBrandId)
            .HasConstraintName("mst_types_brand_id_fkey")
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
    
}