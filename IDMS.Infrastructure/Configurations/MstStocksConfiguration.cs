using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Shared.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IDMS.Infrastructure.Configurations
{
    public class MstStocksConfiguration : IEntityTypeConfiguration<MstStocks>
    {
        public void Configure(EntityTypeBuilder<MstStocks> builder)
        {
            builder.ToTable("mst_stock"); 

            builder.HasKey(e => e.Id)
                .HasName("mst_stock_pkey");

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
                .HasColumnType("numeric(18,2)") 
                .IsRequired();

            builder.Property(e => e.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true);

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
                .HasColumnType("varchar(100)");

            builder.HasOne(e => e.Model)
                .WithMany()
                .HasForeignKey(e => e.ModelId)
                .HasConstraintName("mst_stocks_model_id_fkey");
        }
    }
}