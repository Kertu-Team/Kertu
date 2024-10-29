using Kertu.InteractiveServer.Data.Models.Elements;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Kertu.InteractiveServer.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Element> Elements { get; set; }
        public DbSet<Card> Cards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Element>()
                .HasDiscriminator<string>("type");

            modelBuilder.Entity<Element>()
                .HasOne(ke => ke.ApplicationUser)
                .WithMany(au => au.UserElements)
                .HasForeignKey(ke => ke.ApplicationUserId);

            modelBuilder.Entity<List>()
                .HasMany<Card>();

            modelBuilder.Entity<Board>()
                .HasMany<Element>();
        }
    }
}
