using MatchingGameApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MatchingGameApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<GameItem> GameItems { get; set; }
        public DbSet<Score> Scores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<GameItem>().HasData(
                new GameItem { Id = 1, Name = "Dog", ImageUrl = "/images/dog.png", Category = "Animals" },
                new GameItem { Id = 2, Name = "Cat", ImageUrl = "/images/cat.png", Category = "Animals" },
                new GameItem { Id = 3, Name = "Apple", ImageUrl = "/images/apple.png", Category = "Fruits" },
                new GameItem { Id = 4, Name = "Banana", ImageUrl = "/images/banana.png", Category = "Fruits" }
            );
        }
    }
}
