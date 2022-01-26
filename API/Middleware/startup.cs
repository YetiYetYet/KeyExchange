using System.Globalization;
using API.Middleware.models;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;

namespace API.Middleware;

internal static class Startup
{
    internal static IServiceCollection AddExceptionMiddleware(this IServiceCollection services) =>
        services.AddScoped<ExceptionMiddleware>();

    internal static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app) =>
        app.UseMiddleware<ExceptionMiddleware>();

    internal static IServiceCollection AddRequestLogging(this IServiceCollection services, IConfiguration config)
    {
        if (GetMiddlewareSettings(config).EnableHttpsLogging)
        {
            services.AddSingleton<RequestLoggingMiddleware>();
            services.AddScoped<ResponseLoggingMiddleware>();
        }

        return services;
    }

    internal static IApplicationBuilder UseRequestLogging(this IApplicationBuilder app, IConfiguration config)
    {
        if (GetMiddlewareSettings(config).EnableHttpsLogging)
        {
            app.UseMiddleware<RequestLoggingMiddleware>();
            app.UseMiddleware<ResponseLoggingMiddleware>();
        }

        return app;
    }

    private static MiddlewareSettings GetMiddlewareSettings(IConfiguration config) =>
        config.GetSection(nameof(MiddlewareSettings)).Get<MiddlewareSettings>();

    internal static IServiceCollection AddLocalization(this IServiceCollection services, IConfiguration config)
    {
        services.AddLocalization();

        //services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();

        var middlewareSettings = config.GetSection(nameof(MiddlewareSettings)).Get<MiddlewareSettings>();
        if (middlewareSettings.EnableLocalization)
        {
            services.AddSingleton<LocalizationMiddleware>();
        }

        return services;
    }

    internal static IApplicationBuilder UseLocalization(this IApplicationBuilder app, IConfiguration config)
    {
        app.UseRequestLocalization(new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture(new CultureInfo("en-US"))
        });

        var middlewareSettings = config.GetSection(nameof(MiddlewareSettings)).Get<MiddlewareSettings>();
        if (middlewareSettings.EnableLocalization)
        {
            app.UseMiddleware<LocalizationMiddleware>();
        }

        return app;
    }
}