#region

using MediatR;
using RedFox.Application.DTO;

#endregion

namespace RedFox.Application.Features.Query;

public record GetAllUserWithRelatedQuery : IRequest<IEnumerable<UserDto>>;