using Microsoft.EntityFrameworkCore;
namespace pseven.Models;

public class Structure_meta_context:DbContext

{
    public Structure_meta_context(DbContextOptions<Structure_meta_context> options) 
        : base(options) 
    { 
    }
    public DbSet<Structure_meta> Structure_Metas { get; set; } = null!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Structure_meta>()
        .HasKey(e => e.Structures_id);
    }
}

