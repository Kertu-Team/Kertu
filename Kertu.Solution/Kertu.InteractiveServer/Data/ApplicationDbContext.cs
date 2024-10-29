using Kertu.InteractiveServer.Data.KertuElements;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Kertu.InteractiveServer.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<KertuElement> KertuElements { get; set; }
        public DbSet<KertuCard> KertuCards { get; set; }
        public bool IsCompleted { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<KertuElement>()
                .HasDiscriminator<string>("type");

            modelBuilder.Entity<KertuList>()
                .HasMany<KertuCard>();

            modelBuilder.Entity<KertuBoard>()
                .HasMany<KertuElement>();
        }
    }
}
