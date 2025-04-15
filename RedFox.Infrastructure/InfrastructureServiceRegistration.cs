#region

using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RedFox.Application.Service.Api;
using RedFox.Application.Service.Infrastructure;
using RedFox.Infrastructure.Api;

#endregion

namespace RedFox.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        var baseAddress = new Uri(configuration["Services:ApiUrls:Typicode"] ??
                                  throw new InvalidOperationException("Typicode base url not found"));
        services.AddHttpClient<IUserService, UserService>(client => client.BaseAddress = baseAddress)
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.All
            });
        services.AddDbContext<AppDbContext>((_, builder) =>
            builder.UseSqlite(configuration.GetConnectionString("DefaultConnection")));
        services.AddScoped<IAppDbContext, AppDbContext>();
        return services;
    }
}