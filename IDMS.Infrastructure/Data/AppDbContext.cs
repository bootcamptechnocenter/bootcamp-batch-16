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
        public DbSet<MstUser> MstUsers => Set<MstUser>();
        public DbSet<MstTypes> MstTypes => Set<MstTypes>();
        public DbSet<MstModel> MstModels => Set<MstModel>();
        public DbSet<MstStock> MstStocks => Set<MstStock>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}