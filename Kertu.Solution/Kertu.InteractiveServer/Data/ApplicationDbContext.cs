using Kertu.InteractiveServer.Data.Models.Elements;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Kertu.InteractiveServer.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Element> Elements { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<List> Lists { get; set; }
        public DbSet<Board> Boards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Unable to determine the relationship represented by navigation 'ApplicationUser.UserElements' of type 'List<Element>
            modelBuilder.Entity<Element>()
                .HasOne(ke => ke.ApplicationUser)
                .WithMany(au => au.UserElements)
                .HasForeignKey(ke => ke.ApplicationUserId);

            // Configure Element as an abstract class
            modelBuilder.Entity<Element>()
                .HasDiscriminator<string>("ElementType")
                .HasValue<Card>("Card")
                .HasValue<List>("List")
                .HasValue<Board>("Board");

            modelBuilder.Entity<Card>()
                .HasDiscriminator<string>("CardType")
                .HasValue<Card>("Card")
                .HasValue<TaskCard>("TaskCard");

            // Ignore abstract Element table generation
            modelBuilder.Entity<Element>().ToTable("Elements").HasKey(e => e.Id);
        }
    }
}
