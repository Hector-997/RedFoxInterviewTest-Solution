#region

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RedFox.Domain.Entities;

#endregion

namespace RedFox.Infrastructure.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Username)
            .IsRequired()
            .HasMaxLength(50);

        builder.OwnsOne(u => u.Address, address =>
        {
            address.Property(a => a.Street)
                .IsRequired()
                .HasMaxLength(200);

            address.Property(a => a.Suite)
                .HasMaxLength(100);

            address.Property(a => a.City)
                .IsRequired()
                .HasMaxLength(100);

            address.Property(a => a.Zipcode)
                .IsRequired()
                .HasMaxLength(20);

            address.OwnsOne(a => a.Geo, geo =>
            {
                geo.Property(g => g.Latitude)
                    .IsRequired()
                    .HasMaxLength(50);

                geo.Property(g => g.Longitude)
                    .IsRequired()
                    .HasMaxLength(50);
            });
        });
    }
}
