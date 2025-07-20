using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace HTTP_Methods.Models
{
    public class AppDBContext: DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> dbContext):base(dbContext) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            modelBuilder.Entity<Product>().HasData(
                new Product { Id=1, Name="Laptop", Price=1000.00M, Description="A powerful laptop."},
                new Product { Id=2, Name="Smart Phone", Price=500.00M, Description="A modern laptop."}
            );
        }
        public DbSet<Product> Products { get; set; }
    }

}
