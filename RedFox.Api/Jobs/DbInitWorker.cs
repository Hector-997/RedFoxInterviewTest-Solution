#region

using System.Text.Json;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RedFox.Application.DTO;
using RedFox.Application.Features.Users.Seed;
using RedFox.Application.Service.Api;
using RedFox.Application.Service.Infrastructure;
using RedFox.Domain.ValueObjects;

#endregion

namespace RedFox.Api.Jobs;

public class DbInitWorker(ILogger<DbInitWorker> logger, IServiceProvider scopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            // Aplica migraciones pendientes (crea la DB si no existe)
            await TryInitDb(stoppingToken);
            logger.LogInformation("Database initialized or already up to date.");

            if (await UsersAlreadyExist(stoppingToken))
            {
                logger.LogInformation("Users already exist, skipping seed.");
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
                    GetAddress(x.GetProperty("address")),
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
        var dbContext = scope.ServiceProvider.GetRequiredService<IAppDbContext>();
        var anyUserExists = await dbContext.Users.AnyAsync(ct);
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

    private static AddressDto GetAddress(JsonElement addressElement)
    {
        var street = addressElement.GetProperty("street").GetString() ?? string.Empty;
        var suite = addressElement.GetProperty("suite").GetString() ?? string.Empty;
        var city = addressElement.GetProperty("city").GetString() ?? string.Empty;
        var zipcode = addressElement.GetProperty("zipcode").GetString() ?? string.Empty;
        var geo = GetGeolocation(addressElement.GetProperty("geo"));

        return new AddressDto(street, suite, city, zipcode, geo);
    }

    private static GeolocationDto GetGeolocation(JsonElement geoElement)
    {
        var lat = geoElement.GetProperty("lat").GetString() ?? string.Empty;
        var lng = geoElement.GetProperty("lng").GetString() ?? string.Empty;
        return new GeolocationDto(lat, lng);
    }

    private async Task<Stream> FetchUsers(CancellationToken ct)
    {
        logger.LogInformation("Fetch initial data");
        await using var scope = scopeFactory.CreateAsyncScope();
        var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
        return await userService.GetUsers(ct);
    }

    private async Task TryInitDb(CancellationToken ct)
    {
        logger.LogInformation("Running DB Init Worker");
        await using var scope = scopeFactory.CreateAsyncScope();
        var appDbContext = scope.ServiceProvider.GetRequiredService<IAppDbContext>();
        await appDbContext.Database.MigrateAsync(ct);
    }

    private async Task<bool> UsersAlreadyExist(CancellationToken ct)
    {
        await using var scope = scopeFactory.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IAppDbContext>();

        return await dbContext.Users.AnyAsync(ct);
    }

}