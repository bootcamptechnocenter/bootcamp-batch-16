using Microsoft.EntityFrameworkCore;
using WebApi.Shared.Domain.Entities;

namespace WebApi.Infrastructure.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<MstBrands> MstBrands => Set<MstBrands>();
        public DbSet<MstTypes> MstTypes => Set<MstTypes>();
        public DbSet<MstModels> MstModels => Set<MstModels>();
        public DbSet<MstUsers> MstUsers => Set<MstUsers>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}