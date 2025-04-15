#region

using System.Text.Json;
using MediatR;
using RedFox.Application.DTO;
using RedFox.Application.Features.Query;
using RedFox.Application.Service.Api;
using RedFox.Application.Service.Infrastructure;

#endregion

namespace RedFox.Api.Jobs;

public class DbInitWorker(ILogger<DbInitWorker> logger, IServiceProvider scopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            if (!await TryInitDb(stoppingToken))
            {
                logger.LogInformation("Db already created");
                return;
            }

            await using var userJsonStream = await FetchUsers(stoppingToken);
            var command = await DeserializeJsonStream(userJsonStream, stoppingToken);
            await AddUserToDb(command, stoppingToken);
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "An unexpected error occurred during database maintenance.");
        }
    }

    private static async Task<AddUsersWithRelatedCommand> DeserializeJsonStream(Stream userJsonStream,
        CancellationToken ct)
    {
        using var jsonDocument = await JsonDocument.ParseAsync(userJsonStream, cancellationToken: ct);

        var userCreationDtos = jsonDocument.RootElement.EnumerateArray()
            .Select(x => new UserCreationDto(
                    x.GetProperty("name").GetString() ?? string.Empty,
                    x.GetProperty("username").GetString() ?? string.Empty,
                    x.GetProperty("email").GetString() ?? string.Empty,
                    x.GetProperty("phone").GetString() ?? string.Empty,
                    x.GetProperty("website").GetString() ?? string.Empty,
                    GetCompany(x.GetProperty("company"))
                )
            ).ToList();
        var command = new AddUsersWithRelatedCommand(userCreationDtos);
        return command;
    }

    private async Task AddUserToDb(AddUsersWithRelatedCommand command, CancellationToken ct)
    {
        await using var scope = scopeFactory.CreateAsyncScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var userIds = await mediator.Send(command, ct);
        logger.LogInformation("Db init success, added {P1} new users", userIds.Count());
    }

    private static CompanyDto GetCompany(JsonElement companyElement)
    {
        var name = companyElement.GetProperty("name").GetString() ?? string.Empty;
        var catchPhrase = companyElement.GetProperty("catchPhrase").GetString() ?? string.Empty;
        var bs = companyElement.GetProperty("bs").GetString() ?? string.Empty;
        return new CompanyDto(name, catchPhrase, bs);
    }

    private async Task<Stream> FetchUsers(CancellationToken ct)
    {
        logger.LogInformation("Fetch initial data");
        await using var scope = scopeFactory.CreateAsyncScope();
        var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
        return await userService.GetUsers(ct);
    }

    private async Task<bool> TryInitDb(CancellationToken ct)
    {
        logger.LogInformation("Running DB Init Worker");
        await using var scope = scopeFactory.CreateAsyncScope();
        var appDbContext = scope.ServiceProvider.GetRequiredService<IAppDbContext>();
        return await appDbContext.Database.EnsureCreatedAsync(ct);
    }
}