using MatchingGameApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MatchingGameApp.Data
{

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
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

            // Configure ApplicationUser to ensure compatibility with IdentityUser
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.HasKey(e => e.Id); // Ensure the primary key is configured
            });

            modelBuilder.Entity<GameItem>().HasData(
                // Animals
                new GameItem { Id = 1, Name = "Dog", ImageUrl = "/images/dog.png", Category = "Animals" },
                new GameItem { Id = 2, Name = "Cat", ImageUrl = "/images/cat.png", Category = "Animals" },
                new GameItem { Id = 5, Name = "Rabbit", ImageUrl = "/images/rabbit.png", Category = "Animals" },
                new GameItem { Id = 6, Name = "Horse", ImageUrl = "/images/horse.png", Category = "Animals" },
                new GameItem { Id = 7, Name = "Elephant", ImageUrl = "/images/elephant.png", Category = "Animals" },
                new GameItem { Id = 8, Name = "Lion", ImageUrl = "/images/lion.png", Category = "Animals" },
                new GameItem { Id = 9, Name = "Tiger", ImageUrl = "/images/tiger.png", Category = "Animals" },
                new GameItem { Id = 10, Name = "Bear", ImageUrl = "/images/bear.png", Category = "Animals" },

                // Fruits
                new GameItem { Id = 3, Name = "Apple", ImageUrl = "/images/apple.png", Category = "Fruits" },
                new GameItem { Id = 4, Name = "Banana", ImageUrl = "/images/banana.png", Category = "Fruits" },
                new GameItem { Id = 11, Name = "Cherry", ImageUrl = "/images/cherry.png", Category = "Fruits" },
                new GameItem { Id = 12, Name = "Grapes", ImageUrl = "/images/grapes.png", Category = "Fruits" },
                new GameItem { Id = 13, Name = "Orange", ImageUrl = "/images/orange.png", Category = "Fruits" },
                new GameItem { Id = 14, Name = "Pineapple", ImageUrl = "/images/pineapple.webp", Category = "Fruits" },
                new GameItem { Id = 15, Name = "Strawberry", ImageUrl = "/images/strawberry.webp", Category = "Fruits" },
                new GameItem { Id = 16, Name = "Watermelon", ImageUrl = "/images/watermelon.webp", Category = "Fruits" },

                // Anime (Dragon Ball Characters)
                new GameItem { Id = 17, Name = "Goku", ImageUrl = "/images/goku.png", Category = "Anime" },
                new GameItem { Id = 18, Name = "Chichi", ImageUrl = "/images/Chichi.png", Category = "Anime" },
                new GameItem { Id = 19, Name = "Bills", ImageUrl = "/images/Bills.png", Category = "Anime" },
                new GameItem { Id = 20, Name = "Puar", ImageUrl = "/images/Puar.png", Category = "Anime" },
                new GameItem { Id = 21, Name = "Broly", ImageUrl = "/images/Broly.png", Category = "Anime" },
                new GameItem { Id = 22, Name = "Bulma", ImageUrl = "/images/Bulma.png", Category = "Anime" },
                new GameItem { Id = 23, Name = "Trunks", ImageUrl = "/images/Trunks.png", Category = "Anime" },
                new GameItem { Id = 24, Name = "Krillin", ImageUrl = "/images/Krillin.png", Category = "Anime" }
            );
        }


    }
}

