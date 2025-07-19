#region

using MediatR;
using RedFox.Application.DTO;

#endregion

namespace RedFox.Application.Features.Users.GetAll;

public record GetAllUserWithRelatedQuery : IRequest<IEnumerable<UserDto>>;