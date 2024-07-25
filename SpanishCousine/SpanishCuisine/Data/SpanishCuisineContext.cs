using Microsoft.EntityFrameworkCore;
using SpanishCuisine.Models;


namespace SpanishCuisine.Data
{
    public class SpanishCuisineContext : DbContext
    {
        public SpanishCuisineContext(DbContextOptions<SpanishCuisineContext> option) : base(option)
        {
        
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // establish the primary keys for the many to many relationship
            modelBuilder.Entity<DishIngredient>().HasKey(di => new
            {
                di.DishId, di.IngredientId
 
            });

            // establish the relationships
            modelBuilder.Entity<DishIngredient>()
                .HasOne(di => di.Dish) 
                .WithMany(d => d.DishIngredient) 
                .HasForeignKey(di => di.DishId);

            modelBuilder.Entity<DishIngredient>()
                .HasOne(di => di.Ingredient)
                .WithMany(i => i.DishIngredient)
                .HasForeignKey(di => di.IngredientId);
             
            // add some data to the Cousine
            modelBuilder.Entity<Dish>().HasData(
                new Dish { Id = 1, Name = "Tortilla", Price = 7.50, ImageUrl = "https://www.foodandwine.com/thmb/-h1MufgK3UC4fGnCoeYyF2KRGCU=/1500x0/filters:no_upscale():max_bytes(150000):strip_icc():focal(499x499:501x501)/vegetable-tortilla-XL-RECIPE0917-da3b861d84f74aeabe02a737e9b37e0e.jpg" }
                );

            modelBuilder.Entity<Ingredient>().HasData(
                new Ingredient { Id = 1, Name = "Tomatoes" },
                new Ingredient { Id = 2, Name = "Mozzarelella" }
                );

            modelBuilder.Entity<DishIngredient>().HasData(
                new DishIngredient { DishId=1, IngredientId =1 },
                
                new DishIngredient { DishId=1, IngredientId = 2});

            base.OnModelCreating(modelBuilder);
        }
        //create DbSet for each of the models
        public DbSet<Dish> Dishes { get; set; }

        public DbSet<Ingredient> Ingredients { get; set; }

        public DbSet<DishIngredient> DishIngredients { get; set; }  
    }
}
