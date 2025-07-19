using MediatR;
using RedFox.Application.DTO;

namespace RedFox.Application.Features.Users.Update;

public record UpdateUserWithRelatedCommand(
    int Id, UpdateUserWithRelatedRequest Payload) : IRequest<UserDto>;
