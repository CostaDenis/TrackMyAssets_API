using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrackMyAssets_API.Domain.Entities;

namespace TrackMyAssets_API.Data.Mappings;

public class UserMap : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users")
        .HasKey(x => x.Id);

        builder.Property(x => x.Email)
        .IsRequired()
        .HasColumnName("Email")
        .HasColumnType("VARCHAR")
        .HasMaxLength(255);


        builder.Property(x => x.Password)
        .IsRequired()
        .HasColumnName("Password")
        .HasColumnType("VARCHAR")
        .HasMaxLength(256);

        builder.HasMany(x => x.UserAssets)
        .WithOne(x => x.User)
        .HasForeignKey(x => x.UserId)
        .HasConstraintName("FK_Users_UserId")
        .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.Email, "IX_User_Email")
            .IsUnique();
    }
}