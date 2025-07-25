using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrackMyAssets_API.Domain.Entities;

namespace TrackMyAssets_API.Data.Mappings;

public class AdministratorMap : IEntityTypeConfiguration<Administrator>
{
    public void Configure(EntityTypeBuilder<Administrator> builder)
    {
        builder.ToTable("Administrators")
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
    }
}