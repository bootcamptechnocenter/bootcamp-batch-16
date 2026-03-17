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
            
            builder.HasOne(e => e.Models)
                .WithMany()
                .HasForeignKey(e => e.Model_Id)
                .HasConstraintName("mst_stock_model_id_fky");

            builder.Property(e => e.Model_Id)
                .HasColumnName("model_id")
                .HasColumnType("integer");

            builder.Property(e => e.JumlahStock)
                .HasColumnName("jumlah_stock")
                .HasColumnType("integer")
                .IsRequired();
            
            builder.HasCheckConstraint("CK_mst_stock_jumlah_stock_non_negative", "\"jumlah_stock\" >= 0");
            
            builder.HasCheckConstraint("CK_mst_stock_harga_non_negative", "\"harga\" >= 0");
            
            builder.Property(e => e.Harga)
                .HasColumnName("harga")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamp");

            builder.Property(e => e.CreatedBy)
                .HasColumnName("created_by")
                .HasColumnType("varchar(100)");
            
            builder.Property(e => e.UpdatedAt)
                .HasColumnName("updated_at")
                .HasColumnType("timestamp");            
            
            builder.Property(e => e.UpdatedBy)
                .HasColumnName("updated_by")
                .HasColumnType("varchar(100)");                
            
            builder.Property(e => e.DeletedAt)
                .HasColumnName("deleted_at")
                .HasColumnType("timestamp");

            builder.Property(e => e.DeletedBy)
                .HasColumnName("deleted_by")
                .HasColumnType("varchar(100)");
        }
    }
}