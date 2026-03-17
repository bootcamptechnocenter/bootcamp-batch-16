using IDMS.Shared.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IDMS.Infrastructure.Configurations
{
    public class MstStockConfiguration : IEntityTypeConfiguration<MstStocks>
    {
        public void Configure(EntityTypeBuilder<MstStocks> builder)
        {
            builder.ToTable("mst_stock", tableBuilder =>
            {
                tableBuilder.HasCheckConstraint("ck_mst_stock_jumlah_stock_non_negative", "jumlah_stock >= 0");
                tableBuilder.HasCheckConstraint("ck_mst_stock_harga_non_negative", "harga >= 0");
            });

            builder.HasKey(e => e.Id).HasName("mst_stock_pkey");
            builder.Property(e => e.Id).HasColumnName("id").UseSerialColumn();
            builder.Property(e => e.ModelId).HasColumnName("model_id").HasColumnType("int4").IsRequired();
            builder.Property(e => e.JumlahStock).HasColumnName("jumlah_stock").HasColumnType("int4").IsRequired();
            builder.Property(e => e.Harga).HasColumnName("harga").HasColumnType("numeric(18,2)").IsRequired();
            builder.Property(e => e.CreatedBy).HasColumnName("created_by").HasColumnType("varchar(100)").HasMaxLength(100);
            builder.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp");
            builder.Property(e => e.UpdatedBy).HasColumnName("updated_by").HasColumnType("varchar(100)").HasMaxLength(100);
            builder.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp");
            builder.Property(e => e.DeletedBy).HasColumnName("deleted_by").HasColumnType("varchar(100)").HasMaxLength(100);
            builder.Property(e => e.DeletedAt).HasColumnName("deleted_at").HasColumnType("timestamp");
            builder.HasOne(e => e.Model)
                .WithMany(m => m.Stocks)
                .HasForeignKey(e => e.ModelId)
                .HasConstraintName("mst_stock_model_id_fkey");
        }
    }
}