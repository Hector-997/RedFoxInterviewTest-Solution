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
            address.Property(a => a.Street).HasColumnName("Street");
            address.Property(a => a.Suite).HasColumnName("Suite");
            address.Property(a => a.City).HasColumnName("City");
            address.Property(a => a.Zipcode).HasColumnName("Zipcode");

            address.OwnsOne(a => a.Geo, geo =>
            {
                geo.Property(g => g.Latitude).HasColumnName("GeoLat");
                geo.Property(g => g.Longitude).HasColumnName("GeoLng");
            });
        });
    }
}
