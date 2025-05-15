using GrӧkTube.Entities;
using Microsoft.EntityFrameworkCore;

namespace GrӧkTube.Repository
{
    public class GrokRepository : DbContext
    {

        public GrokRepository(DbContextOptions<GrokRepository> options):base(options) { }

        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");  

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.Login).HasColumnName("login");
                entity.Property(e => e.HashPassword).HasColumnName("hashpassword");
                entity.Property(e => e.PhoneNumber).HasColumnName("phonenumber");
                entity.Property(e => e.Race).HasColumnName("race");
                entity.Property(e => e.AvatarUrl).HasColumnName("avatarurl");
            });

        }
    }
}
