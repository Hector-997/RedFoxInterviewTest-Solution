#region

using AutoMapper;
using global::RedFox.Application.Features.Users.Create;
using global::RedFox.Application.Service.Infrastructure;
using global::RedFox.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using RedFox.Application.DTO;

#endregion

namespace RedFox.Application.Features.Users.Update;

public class UpdateUserWithRelatedCommandHandler(
    IAppDbContext context,
    IMapper mapper,
    ILogger<UpdateUserWithRelatedCommandHandler> logger
) : IRequestHandler<UpdateUserWithRelatedCommand, UserDto?>
{
    public async Task<UserDto?> Handle(
        UpdateUserWithRelatedCommand command, 
        CancellationToken cancellationToken)
    {
        var user = await context.Users.FindAsync(command.Id, cancellationToken);

        if (user is null) return null;

        mapper.Map(command.Payload, user);

        context.Users.Update(user);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation($"User with id: {user.Id} updated sucessfully");

        return mapper.Map<UserDto>(user);
    }
}
