#region

using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using RedFox.Application.Service.Infrastructure;
using RedFox.Domain.Entities;

#endregion

namespace RedFox.Application.Features.Users.Create;

public class AddUserWithRelatedCommandHandler(
    IAppDbContext context,
    IMapper mapper,
    ILogger<AddUserWithRelatedCommandHandler> logger
) : IRequestHandler<AddUserWithRelatedCommand, int>
{
    public async Task<int> Handle(AddUserWithRelatedCommand request, CancellationToken ct)
    {
        await using var transaction = await context.Database.BeginTransactionAsync(ct);
        try
        {
            var user = mapper.Map<User>(request.User);
            await context.Users.AddAsync(user, ct);
            await context.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
            return user.Id;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Batch user creation failed");
            await transaction.RollbackAsync(ct);
            throw;
        }
    }
}