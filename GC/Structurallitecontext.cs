using Microsoft.EntityFrameworkCore;
namespace pseven.Models;

public class Structurallitecontext :DbContext
{
    public Structurallitecontext(DbContextOptions<Structurallitecontext> options)
        : base(options)
    {
     }
    public DbSet<StructureLite_call> StructureLite_Calls { get; set; } = null!;
}
