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
            builder.ToTable("mst_stocks");

            builder.HasKey(e => e.Id)
                .HasName("mst_stocks_pkey");

            builder.HasOne(e => e.Model)
                .WithMany()
                .HasForeignKey(e => e.MstModelId)
                .HasConstraintName("mst_stocks_model_id_fkey")
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(e => e.Id)
                .HasColumnName("id")
                .UseSerialColumn();

            builder.Property(e => e.TotalStock)
                .HasColumnName("total_stock")
                .HasColumnType("int")
                .IsRequired();

            builder.Property(e => e.Price)
                .HasColumnName("price")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(e => e.MstModelId)
                .HasColumnName("model_id")
                .HasColumnType("int")
                .IsRequired();

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