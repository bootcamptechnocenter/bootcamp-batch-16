using IDMS.Shared.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IDMS.Infrastructure.Configurations
{
  public class MstModelsConfiguration : IEntityTypeConfiguration<MstModels>
  {
    public void Configure(EntityTypeBuilder<MstModels> builder)
    {
      builder.ToTable("mst_models", tableBuilder => tableBuilder.ExcludeFromMigrations());

      builder.HasKey(e => e.Id).HasName("mst_models_pkey");
      builder.Property(e => e.Id).HasColumnName("id").UseSerialColumn();
      builder.Property(e => e.TypeId).HasColumnName("type_id").HasColumnType("int4").IsRequired();
      builder.Property(e => e.Code).HasColumnName("code").HasColumnType("varchar(10)").HasMaxLength(10).IsRequired();
      builder.Property(e => e.Name).HasColumnName("name").HasColumnType("varchar(100)").HasMaxLength(100).IsRequired();
      builder.Property(e => e.Year).HasColumnName("year").HasColumnType("int4").IsRequired();
      builder.Property(e => e.IsActive).HasColumnName("is_active").HasDefaultValue(true);
      builder.Property(e => e.CreatedBy).HasColumnName("created_by").HasColumnType("varchar(100)").HasMaxLength(100);
      builder.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp");
      builder.Property(e => e.UpdatedBy).HasColumnName("updated_by").HasColumnType("varchar(100)").HasMaxLength(100);
      builder.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp");
      builder.Property(e => e.DeletedBy).HasColumnName("deleted_by").HasColumnType("varchar(100)").HasMaxLength(100);
      builder.Property(e => e.DeletedAt).HasColumnName("deleted_at").HasColumnType("timestamp");
      builder.HasOne(e => e.Type)
          .WithMany(t => t.Models)
          .HasForeignKey(e => e.TypeId)
          .HasConstraintName("mst_models_type_id_fkey");
    }



  }
}