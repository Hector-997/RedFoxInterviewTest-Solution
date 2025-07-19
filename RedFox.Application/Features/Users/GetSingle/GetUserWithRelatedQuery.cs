#region

using MediatR;
using RedFox.Application.DTO;

#endregion

namespace RedFox.Application.Features.Users.GetSingle;

public record GetUserWithRelatedQuery(int UserId) : IRequest<UserDto>;