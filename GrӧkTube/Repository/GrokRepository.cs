using GrӧkTube.Entities;
using Microsoft.EntityFrameworkCore;

namespace GrӧkTube.Repository
{
    public class GrokRepository : DbContext
    {

        public GrokRepository(DbContextOptions<GrokRepository> options):base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
