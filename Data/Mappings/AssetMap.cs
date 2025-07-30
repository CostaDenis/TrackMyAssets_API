using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrackMyAssets_API.Domain.Entities;

namespace TrackMyAssets_API.Data.Mappings;

public class AssetMap : IEntityTypeConfiguration<Asset>
{
    public void Configure(EntityTypeBuilder<Asset> builder)
    {
        builder.ToTable("Assets")
        .HasKey(x => x.Id);

        builder.Property(x => x.Name)
        .IsRequired()
        .HasColumnName("Name")
        .HasColumnType("NVARCHAR")
        .HasMaxLength(60);

        builder.Property(x => x.Symbol)
        .HasColumnName("Symbol")
        .HasColumnType("NVARCHAR")
        .HasMaxLength(8);

        builder.Property(x => x.Type)
        .IsRequired()
        .HasColumnName("Type")
        .HasConversion<string>();

        builder.HasMany(x => x.UserAssets)
        .WithOne(x => x.Asset)
        .HasForeignKey(x => x.AssetId)
        .HasConstraintName("FK_Assets_AssetId")
        .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.Name, "IX_Asset_Name")
            .IsUnique();

    }
}
