#region

using MediatR;
using RedFox.Application.DTO;

#endregion

namespace RedFox.Application.Features.Users.Seed;

public record AddUsersWithRelatedCommand(IEnumerable<UserCreationDto> Users) : IRequest<IEnumerable<int>>;