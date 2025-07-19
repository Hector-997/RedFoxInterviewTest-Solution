#region

using MediatR;
using RedFox.Application.DTO;

#endregion

namespace RedFox.Application.Features.Users.Create;

public record AddUsersWithRelatedCommand(IEnumerable<UserCreationDto> Users) : IRequest<IEnumerable<int>>;