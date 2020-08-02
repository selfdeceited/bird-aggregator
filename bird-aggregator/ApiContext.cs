using Microsoft.EntityFrameworkCore;
using birds.Domain;
 
namespace birds
{
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options)
            : base(options)
        {
        }
 
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Bird> Birds { get; set; }
        public DbSet<Location> Locations { get; set; }
    }
}