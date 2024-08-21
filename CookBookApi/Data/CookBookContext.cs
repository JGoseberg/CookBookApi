﻿using CookBookApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CookBookApi.Data
{
    public class CookBookContext : DbContext 
    {
        public CookBookContext(DbContextOptions<CookBookContext> options)
            : base (options)
        {
        }

        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Recipe>()
                .HasMany(e => e.Ingredients)
                .WithMany(e => e.Recipes);

            
        }
    }
}
