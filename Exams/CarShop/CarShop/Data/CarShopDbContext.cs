﻿using CarShop.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CarShop.Data
{
    public class CarShopDbContext : DbContext
    {
        public DbSet<Car> Cars { get; set; }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=CarShop;Integrated Security=True;");
            }
        }
    }
}

