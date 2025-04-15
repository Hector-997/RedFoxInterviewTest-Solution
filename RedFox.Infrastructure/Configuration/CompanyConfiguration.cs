#region

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RedFox.Domain.Entities;

#endregion

namespace RedFox.Infrastructure.Configuration;

public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.HasKey(c => c.UserId);
        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
    }
}