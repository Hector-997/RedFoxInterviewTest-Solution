using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using RedFox.Application.Service.Infrastructure;

namespace RedFox.Application.Features.Users.Delete;

public class DeleteUserCommandHandler(
    IAppDbContext context,
    ILogger<DeleteUserCommandHandler> logger)
    : IRequestHandler<DeleteUserCommand, bool>
{
    public async Task<bool> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        var user = await context.Users.FindAsync(command.Id, cancellationToken);

        if (user is null) return false;

        context.Users.Remove(user);

        await context.SaveChangesAsync();

        logger.LogInformation($"User with id: {command.Id} was deleted successfully");

        return true;
    }
}
