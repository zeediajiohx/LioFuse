using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
namespace pseven.Models.structure;
public class Structure_LiteContext:DbContext
{
    public Structure_LiteContext(DbContextOptions<Structure_LiteContext> options) : 
        base(options) { }
    public DbSet<Structure_Lite> Structure_Lites { get; set; } = null!;
}
public class StructureContext : DbContext
{
    public StructureContext(DbContextOptions<StructureContext> options) : base(options) { }
    public DbSet<Structure> Structures { get; set; } = null!;
}











