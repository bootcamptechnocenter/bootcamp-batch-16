using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Shared.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IDMS.Infrastructure.Configurations
{
    public class MstUserConfiguration: IEntityTypeConfiguration<MstUser>
    {
        public void Configure(EntityTypeBuilder<MstUser> builder)
        {
            builder.ToTable("mst_user");

            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.Id)
                .HasColumnName("id")
                .UseSerialColumn();

            builder.Property(x => x.Email)
                .HasColumnName("email")
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(x => x.Password)
                .HasColumnName("password")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.FullName)
                .HasColumnName("full_name")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.CreatedBy)
                .HasColumnName("created_by")
                .HasMaxLength(100)
                .HasColumnType("varchar(100)");

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamp");

            builder.Property(x => x.UpdatedBy)
                .HasColumnName("updated_by")
                .HasMaxLength(100)
                .HasColumnType("varchar(100)");

            builder.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at")
                .HasColumnType("timestamp"); 

            builder.Property(x => x.DeletedBy)
                .HasColumnName("deleted_by")
                .HasMaxLength(100)
                .HasColumnType("varchar(100)");

            builder.Property(x => x.DeletedAt)
                .HasColumnName("deleted_at")
                .HasColumnType("timestamp");
        }
    }
}