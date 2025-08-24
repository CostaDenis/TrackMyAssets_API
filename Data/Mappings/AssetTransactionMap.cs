using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrackMyAssets_API.Domain.Entities;

namespace TrackMyAssets_API.Data.Mappings;

public class AssetTransactionMap : IEntityTypeConfiguration<AssetTransaction>
{
    public void Configure(EntityTypeBuilder<AssetTransaction> builder)
    {
        builder.ToTable("AssetTransactions")
        .HasKey(x => x.Id);

        builder.Property(x => x.UserId)
        .HasColumnName("UserId");

        builder.Property(x => x.AssetId)
        .IsRequired()
        .HasColumnName("AssetId");

        builder.Property(x => x.UnitsChanged)
        .IsRequired()
        .HasColumnName("UnitsChanged")
        .HasColumnType("DECIMAL(18,8)");

        builder.Property(x => x.Date)
        .IsRequired()
        .HasColumnName("Date")
        .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(x => x.Note)
        .HasColumnType("NVARCHAR")
        .HasMaxLength(500);

        builder.Property(x => x.Type)
        .IsRequired()
        .HasMaxLength(20)
        .HasColumnType("VARCHAR");

        builder.HasOne(x => x.Asset)
        .WithMany()
        .HasForeignKey(x => x.AssetId)
        .HasConstraintName("FK_AssetTransactions_AssetId")
        .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.User)
        .WithMany()
        .HasForeignKey(x => x.UserId)
        .HasConstraintName("FK_AssetTransactions_UserId")
        .OnDelete(DeleteBehavior.Cascade);
    }
}
