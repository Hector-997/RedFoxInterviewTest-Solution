#region

using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RedFox.Application.DTO;
using RedFox.Application.Features.Query;
using RedFox.Application.Service.Infrastructure;

#endregion

namespace RedFox.Application.Features.Handler;

public class GetUserWithRelatedHandler(IAppDbContext context, IMapper mapper) :
    IRequestHandler<GetUserWithRelatedQuery, UserDto>
{
    public async Task<UserDto> Handle(GetUserWithRelatedQuery request, CancellationToken ct)
    {
        var user = await context.Users
            .Include(u => u.Company)
            .Where(u => u.Id == request.UserId)
            .FirstOrDefaultAsync(ct);

        var userDto = mapper.Map<UserDto>(user);

        return userDto;
    }
}