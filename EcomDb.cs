using Microsoft.EntityFrameworkCore;

class EcomDb : DbContext
{
    public EcomDb(DbContextOptions<EcomDb> options) : base(options) { }

    public DbSet<Sale> Sales => Set<Sale>();

    public DbSet<Product> Products => Set<Product>();

    public DbSet<Category> Categories => Set<Category>();
}