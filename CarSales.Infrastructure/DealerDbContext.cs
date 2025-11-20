using CarSales.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Infrastructure
{
    public class DealerDbContext : DbContext
    {
        public DealerDbContext(DbContextOptions<DealerDbContext> options) : base(options)
        {
        }

        public DbSet<CarBrand> CarBrands { get; set; }
        public DbSet<CarModel> CarModels { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CarBrand>()
                .HasIndex(b => b.Name)
                .IsUnique();

            modelBuilder.Entity<CarModel>()
                .HasIndex(c => c.Name)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
