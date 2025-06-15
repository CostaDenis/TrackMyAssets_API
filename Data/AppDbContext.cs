using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TrackMyAssets_API.Domain.Entities;

namespace TrackMyAssets_API.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) :
            base(options)
        {
            // _configuration = configuration;
        }

        // private readonly IConfiguration _configuration;

        public DbSet<User> Users { get; set; } = default!;
        public DbSet<Administrator> Administrators { get; set; } = default!;
        public DbSet<Asset> Assets { get; set; } = default!;
        public DbSet<AssetTransaction> AssetTransactions { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Administrator>().HasData(
                new Administrator
                {
                    Email = "adm@teste.com",
                    Password = "123456"
                }
            );
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // if (!optionsBuilder.IsConfigured)
            // {
            //     var ConnectionString = _configuration.GetConnectionString("DefaultConnection");

            //     if (!string.IsNullOrEmpty(ConnectionString))
            //         optionsBuilder.UseSqlServer(ConnectionString);

            // }
        }

    }
}