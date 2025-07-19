using MediatR;

namespace RedFox.Application.Features.Users.Delete;

public record DeleteUserCommand(int Id) : IRequest<bool>;
