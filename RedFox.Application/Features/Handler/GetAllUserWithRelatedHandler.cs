#region

using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RedFox.Application.DTO;
using RedFox.Application.Features.Query;
using RedFox.Application.Service.Infrastructure;

#endregion

namespace RedFox.Application.Features.Handler;

public class GetAllUserWithRelatedHandler(
    IAppDbContext context,
    IMapper mapper
) : IRequestHandler<GetAllUserWithRelatedQuery, IEnumerable<UserDto>>
{
    public async Task<IEnumerable<UserDto>> Handle(GetAllUserWithRelatedQuery request, CancellationToken ct)
    {
        var user = await context.Users
            .Include(u => u.Company)
            .ToListAsync(ct);

        return mapper.Map<IEnumerable<UserDto>>(user);
    }
}