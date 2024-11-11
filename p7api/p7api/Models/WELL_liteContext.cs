using Microsoft.EntityFrameworkCore;  
namespace pseven.Models.well;

public class WELL_liteContext:DbContext

{
    public WELL_liteContext(DbContextOptions<WELL_liteContext> options) 
        : base(options) 
    { 

    }
    public DbSet<WELL_lite> Well_lites { get; set; } = null!;
}
public class WellContext : DbContext
{
    public WellContext(DbContextOptions<WellContext> options)
        : base(options)
    {
    }
    public DbSet<Well> Wellitems { get; set; } = null!;

}
//public class StructureR_def_Context : DbContext

//{
//    public StructureR_def_Context(DbContextOptions<StructureR_def_Context> options)
//        : base(options)
//    {
//    }
//    public DbSet<StructureR_def> StructureR_def { get; set; } = null!;
//}
