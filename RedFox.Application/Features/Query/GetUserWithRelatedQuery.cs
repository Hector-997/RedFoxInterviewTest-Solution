#region

using MediatR;
using RedFox.Application.DTO;

#endregion

namespace RedFox.Application.Features.Query;

public record GetUserWithRelatedQuery(int UserId) : IRequest<UserDto>;