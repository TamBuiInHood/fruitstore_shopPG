using Contracts.Domains.Interfaces;
using Microsoft.EntityFrameworkCore;
using Product.API.Entities;

namespace Product.API.Presistance
{
    public class ProductContext : DbContext
    {

        public ProductContext(DbContextOptions<ProductContext> opnstions) : base(opnstions)
        {
        }

        public DbSet<Entities.CatalogProduct> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CatalogProduct>().HasIndex(x => x.No)
                .IsUnique();
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var modified = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified ||
                e.State == EntityState.Added ||
                e.State == EntityState.Deleted);

            foreach (var item in modified)
            {
                switch (item.State)
                {
                    case EntityState.Added:
                        if (item.Entity is IDateTracking addEntitiy)
                        {
                            addEntitiy.CreateDate = DateTime.Now;
                            item.State = EntityState.Added;
                        }
                        break;
                    case EntityState.Modified:
                        Entry(item.Entity).Property("Id").IsModified = false;
                        if (item.Entity is IDateTracking modifiedEntity)
                        {
                            modifiedEntity.LastModifiedDate = DateTime.UtcNow;
                            item.State = EntityState.Modified;
                        }
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
