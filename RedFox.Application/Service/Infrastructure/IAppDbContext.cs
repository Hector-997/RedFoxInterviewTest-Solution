#region

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using RedFox.Domain.Entities;

#endregion

namespace RedFox.Application.Service.Infrastructure;

public interface IAppDbContext
{
    DbSet<User> Users { get; }

    DbSet<Company> Companies { get; }

    /// <inheritdoc cref="DbContext.Database" />
    DatabaseFacade Database { get; }

    /// <inheritdoc cref="DbContext.SaveChangesAsync(System.Threading.CancellationToken)" />
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}