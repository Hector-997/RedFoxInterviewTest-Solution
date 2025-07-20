namespace RedFox.Api.Config;

public static class CorsExtensions
{
    public const string DevelopmentPolicy = "DevCorsPolicy";

    public static IServiceCollection AddCustomCors(this IServiceCollection services, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            services.AddCors(options =>
            {
                options.AddPolicy(DevelopmentPolicy, policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });
        }

        return services;
    }

    public static IApplicationBuilder UseCustomCors(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseCors(DevelopmentPolicy);
        }

        return app;
    }
}

