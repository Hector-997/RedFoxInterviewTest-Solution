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
        var user = mapper.Map<User>(request.Payload);
        await context.Users.AddAsync(user, ct);
        await context.SaveChangesAsync(ct);
        logger.LogInformation($"User created: {user.Id}");
        return user.Id;
    }
}