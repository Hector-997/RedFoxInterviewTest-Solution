﻿#region

using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using RedFox.Application.Service.Infrastructure;
using RedFox.Domain.Entities;

#endregion

namespace RedFox.Application.Features.Users.Create;

public class AddUsersWithRelatedCommandHandler(
    IAppDbContext context,
    IMapper mapper,
    ILogger<AddUsersWithRelatedCommandHandler> logger
) : IRequestHandler<AddUsersWithRelatedCommand, IEnumerable<int>>
{
    public async Task<IEnumerable<int>> Handle(AddUsersWithRelatedCommand request, CancellationToken ct)
    {
        await using var transaction = await context.Database.BeginTransactionAsync(ct);
        try
        {
            var users = mapper.Map<List<User>>(request.Users);
            await context.Users.AddRangeAsync(users, ct);
            await context.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
            return users.Select(u => u.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Batch user creation failed");
            await transaction.RollbackAsync(ct);
            throw;
        }
    }
}