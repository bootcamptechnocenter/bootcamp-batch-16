using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDMS.Shared.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IDMS.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<MstBrands> MstBrands => Set<MstBrands>();
        public DbSet<MstTypes> MstTypes => Set<MstTypes>();
        public DbSet<MstUsers> MstUsers => Set<MstUsers>();
        public DbSet<MstModels> MstModels => Set<MstModels>();
        public DbSet<MstStocks> MstStocks => Set<MstStocks>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}