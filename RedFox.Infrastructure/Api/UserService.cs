#region

using Microsoft.Extensions.Logging;
using RedFox.Application.Service.Api;

#endregion

namespace RedFox.Infrastructure.Api;

public class UserService(HttpClient httpClient, ILogger<UserService> logger) : IUserService
{
    public async Task<Stream> GetUsers(CancellationToken ct)
    {
        try
        {
            var res = await httpClient.GetAsync("users", ct);
            res.EnsureSuccessStatusCode();
            return await res.Content.ReadAsStreamAsync(ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Get users failed");
            throw;
        }
    }
}