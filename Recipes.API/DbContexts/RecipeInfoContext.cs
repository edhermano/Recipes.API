using Microsoft.EntityFrameworkCore;
using Recipes.API.Entities;
using Recipes.API.Models;

namespace Recipes.API.DbContexts
{
    public class RecipeInfoContext : DbContext
    {
        public DbSet<Recipe> Recipes { get; set; }

        public DbSet<Ingredient> Ingredients { get; set; }

        public RecipeInfoContext(DbContextOptions<RecipeInfoContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Recipe>()
                .HasData(
                new Recipe()
                {
                    Id = 1,
                    Name = "Cajun Meatloaf",
                    Directions = "Mix bake serve",
                    Difficulty = "Easy",
                    Rating = 5
                },
                new Recipe()
                {
                    Id = 2,
                    Name = "Mexican Lasagna",
                    Directions = "Mix bake serve",
                    Difficulty = "Easy",
                    Rating = 5
                },
                new Recipe()
                {
                    Id = 3,
                    Name = "Enchilades",
                    Directions = "Mix Roll bake serve",
                    Difficulty = "Easy",
                    Rating = 5
                });

            modelBuilder.Entity<Ingredient>()
                .HasData(
                new Ingredient()
                {
                    Id = 1,
                    Name = "Ground Beef",
                    Qty = 2,
                    Measurement = "lbs",
                    RecipeId = 1

                },
                new Ingredient()
                {
                    Id = 2,
                    Name = "Egg",
                    Qty = 2,
                    RecipeId = 1
                },
                new Ingredient()
                {
                    Id = 3,
                    Name = "Bread Crumbs",
                    Qty = 1,
                    Measurement = "cup",
                    RecipeId = 1
                },
                new Ingredient()
                {
                    Id = 4,
                    Name = "Lasagna Noodles",
                    Qty = 8,
                    Measurement = "oz",
                    RecipeId = 2
                },
                new Ingredient()
                {
                    Id = 5,
                    Name = "Chorizo",
                    Qty = 8,
                    Measurement = "oz",
                    RecipeId = 2
                },
                new Ingredient()
                {
                    Id = 6,
                    Name = "Ground Beef",
                    Qty = 8,
                    Measurement = "oz",
                    RecipeId = 2
                },
                new Ingredient()
                {
                    Id = 7,
                    Name = "Tortillas",
                    Qty = 12,
                    RecipeId = 3
                },
                new Ingredient()
                {
                    Id = 8,
                    Name = "Enchilada Sauce",
                    Qty = 16,
                    Measurement = "oz",
                    RecipeId = 3
                },
                new Ingredient()
                {
                    Id = 9,
                    Name = "Cheese",
                    Qty = 16,
                    Measurement = "oz",
                    RecipeId = 3
                },
                new Ingredient()
                {
                    Id = 10,
                    Name = "Chicken",
                    Qty = 12,
                    Measurement = "oz",
                    RecipeId = 3
                }
                );

            base.OnModelCreating(modelBuilder);
        }
    }
}
