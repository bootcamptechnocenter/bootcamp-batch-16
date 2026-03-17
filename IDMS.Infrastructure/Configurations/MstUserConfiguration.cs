using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using IDMS.Shared.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IDMS.Infrastructure.Configurations
{
    public class MstUserConfiguration : IEntityTypeConfiguration<MstUsers>
    {
        public void Configure(EntityTypeBuilder<MstUsers> builder)
        {
            builder.ToTable("mst_users");
            builder.HasKey(e => e.Id).HasName("pk_mst_user");
            builder.Property(e => e.Id).HasColumnName("id").UseSerialColumn();
            builder.Property(e => e.Email).HasColumnName("email").HasColumnType("varchar(50)").IsRequired().HasMaxLength(50);
            builder.Property(e => e.Password).HasColumnName("password").HasColumnType("varchar(255)").IsRequired().HasMaxLength(255);
            builder.Property(e => e.FullName).HasColumnName("full_name").HasColumnType("varchar(100)").IsRequired().HasMaxLength(100);
            builder.Property(e => e.CreatedBy).HasColumnName("created_by").HasColumnType("varchar(100)").HasMaxLength(100);
            builder.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp");
            builder.Property(e => e.UpdatedBy).HasColumnName("updated_by").HasColumnType("varchar(100)").HasMaxLength(100);
            builder.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp");
            builder.Property(e => e.DeletedBy).HasColumnName("deleted_by").HasColumnType("varchar(100)").HasMaxLength(100);
            builder.Property(e => e.DeletedAt).HasColumnName("deleted_at").HasColumnType("timestamp");
        }
    }
}