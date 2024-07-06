using Microsoft.EntityFrameworkCore;
using WeightManager.Model.Models;

namespace WeightManager.Models;

public class WeightManagerContext : DbContext
{
    public WeightManagerContext(DbContextOptions<WeightManagerContext> options) : base(options) 
    { 
    }

    public DbSet<User> User { get; set; } = null!;
    public DbSet<WeightEntry> WeightEntry { get; set; } = null!;
}

