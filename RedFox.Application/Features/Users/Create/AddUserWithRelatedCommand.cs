using MediatR;
using RedFox.Application.DTO;

namespace RedFox.Application.Features.Users.Create;

public record AddUserWithRelatedCommand(AddUserWithRelatedRequest Payload) : IRequest<int>;
