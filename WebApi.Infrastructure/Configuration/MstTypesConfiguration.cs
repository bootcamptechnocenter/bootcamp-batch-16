using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Shared.Domain.Entities;

namespace WebApi.Infrastructure.Configuration
{
    public class MstTypesConfiguration : IEntityTypeConfiguration<MstTypes>
    {
        public void Configure(EntityTypeBuilder<MstTypes> builder)
        {
            builder.ToTable("mst_types");

            builder.Property(e => e.Id)
                .HasColumnName("id")
                .UseSerialColumn();
            
            builder.HasKey(e => e.Id)
                .HasName("mst_types_pkey");

            builder.Property(e => e.BrandId)
                .HasColumnName("brand_id")
                .HasColumnType("integer")
                .IsRequired();

            builder.HasOne<MstBrands>()
                .WithMany()
                .HasForeignKey(e => e.BrandId)
                .HasConstraintName("fk_mst_types_mst_brands")
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(e => e.Code)
                .HasColumnName("code")
                .HasColumnType("varchar(10)")
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(e => e.Name)
                .HasColumnName("name")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.IsActive)
                .HasColumnName("is_active")
                .HasColumnType("boolean")
                .HasDefaultValue(true);

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