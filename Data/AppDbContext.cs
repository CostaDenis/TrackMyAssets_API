using Microsoft.EntityFrameworkCore;
using TrackMyAssets_API.Data.Mappings;
using TrackMyAssets_API.Domain.Entities;

namespace TrackMyAssets_API.Data;

public class AppDbContext : DbContext
{

    public AppDbContext(DbContextOptions<AppDbContext> options) :
        base(options)
    {

    }

    public DbSet<User> Users { get; set; } = default!;
    public DbSet<Administrator> Administrators { get; set; } = default!;
    public DbSet<Asset> Assets { get; set; } = default!;
    public DbSet<AssetTransaction> AssetTransactions { get; set; } = default!;
    public DbSet<UserAsset> UserAssets { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Administrator>().HasData(
            new Administrator
            {
                Email = "adm@teste.com",
                Password = "AQAAAAIAAYagAAAAEC9OuKj8Axx4BT2qIe47xaon8XM1Nyv2HW38v30wSNL+JAmH4c3pl9ufIIX0bSoVJA=="
            }
        );

        modelBuilder.ApplyConfiguration(new AdministratorMap());
        modelBuilder.ApplyConfiguration(new AssetMap());
        modelBuilder.ApplyConfiguration(new AssetTransactionMap());
        modelBuilder.ApplyConfiguration(new UserAssetMap());
        modelBuilder.ApplyConfiguration(new UserMap());
    }

}
