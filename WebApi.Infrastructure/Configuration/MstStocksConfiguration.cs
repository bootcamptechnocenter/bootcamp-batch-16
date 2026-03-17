using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Shared.Domain.Entities;

namespace WebApi.Infrastructure.Configuration
{
    public class MstStocksConfiguration : IEntityTypeConfiguration<MstStocks>
    {
        public void Configure(EntityTypeBuilder<MstStocks> builder)
        {
            builder.ToTable("mst_stocks");

            builder.Property(e => e.Id)
                .HasColumnName("id")
                .UseSerialColumn();

            builder.HasKey(e => e.Id)
                .HasName("mst_stocks_pkey");

            builder.Property(e => e.ModelId)
                .HasColumnName("model_id")
                .HasColumnType("integer")
                .IsRequired();

            builder.HasOne<MstModels>()
                .WithMany()
                .HasForeignKey(e => e.ModelId)
                .HasConstraintName("fk_mst_stocks_mst_models")
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(e => e.Stock)
                .HasColumnName("stock")
                .HasColumnType("integer")
                .IsRequired();

            builder.Property(e => e.Price)
                .HasColumnName("price")
                .HasColumnType("numeric(18,2)")
                .IsRequired();

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
