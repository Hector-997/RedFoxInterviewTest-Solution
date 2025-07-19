#region

using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RedFox.Application.Behaviors;
using RedFox.Application.Features.Users.Create;
using RedFox.Application.Profiles;

#endregion

namespace RedFox.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile));
        
        services.AddMediatR(cfg =>
        {
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
        });

        //services.AddValidatorsFromAssembly(typeof(ApplicationServiceRegistration).Assembly);

        services.AddValidatorsFromAssemblyContaining<AddUserWithRelatedRequestValidator>();

        return services;
    }
}