using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrackMyAssets_API.Domain.Entities;

namespace TrackMyAssets_API.Data.Mappings;

public class UserAssetMap : IEntityTypeConfiguration<UserAsset>
{
    public void Configure(EntityTypeBuilder<UserAsset> builder)
    {
        builder.ToTable("UserAssets")
        .HasKey(x => x.Id);

        builder.Property(x => x.UserId)
        .IsRequired()
        .HasColumnName("UserId");

        builder.Property(x => x.AssetId)
        .IsRequired()
        .HasColumnName("AssetId");

        builder.Property(x => x.Units)
        .IsRequired()
        .HasColumnType("DECIMAL(18,2)")
        .HasDefaultValue(0);

        builder.HasOne(x => x.User)
        .WithMany(x => x.UserAssets)
        .HasForeignKey(x => x.UserId)
        .HasConstraintName("FK_UserAssets_UserId")
        .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Asset)
        .WithMany(x => x.UserAssets)
        .HasForeignKey(x => x.AssetId)
        .HasConstraintName("FK_UserAssets_AssetId")
        .OnDelete(DeleteBehavior.Cascade);
    }
}
