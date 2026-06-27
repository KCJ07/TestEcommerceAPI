using Microsoft.EntityFrameworkCore;

class EcomDb : DbContext
{
    public EcomDb(DbContextOptions<Shiftdb> options) : base(options) { }

    public DbSet<Shift> Shifts => Set<Shift>();
}