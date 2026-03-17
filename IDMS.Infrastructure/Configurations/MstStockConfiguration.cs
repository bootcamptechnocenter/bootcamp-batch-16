using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Shared.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IDMS.Infrastructure.Configurations
{
    public class MstStockConfiguration : IEntityTypeConfiguration<MstStock>
    {
        public void Configure(EntityTypeBuilder<MstStock> builder)
        {
            builder.ToTable("mst_stocks");
            builder.HasKey(e => e.Id)
                .HasName("mst_stocks_pkey");

            builder.Property(e => e.Id)
                .HasColumnName("id")
                .UseSerialColumn();

            builder.Property(e => e.ModelId)
                .HasColumnName("model_id")
                .IsRequired();

            builder.Property(e => e.JumlahStock)
                .HasColumnName("jumlah_stock")
                .IsRequired();

            builder.Property(e => e.Harga)
                .HasColumnName("harga")
                .IsRequired();

            builder.Property(e => e.CreatedBy)
               .HasColumnName("created_by")
               .HasColumnType("varchar(100)")
               .HasMaxLength(100);

            builder.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamp");

            builder.Property(e => e.UpdatedBy)
                .HasColumnName("updated_by")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            builder.Property(e => e.UpdatedAt)
                .HasColumnName("updated_at")
                .HasColumnType("timestamp");

            builder.Property(e => e.DeletedAt)
                .HasColumnName("deleted_at")
                .HasColumnType("timestamp");

            builder.Property(e => e.DeletedBy)
                .HasColumnName("deleted_by")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100);

            builder.HasOne(e => e.Model)
            .WithMany()
            .HasForeignKey(e => e.ModelId)
            .HasConstraintName("fk_mst_stocks_mst_models");
        }
    }
}