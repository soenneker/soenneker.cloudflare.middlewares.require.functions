using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Soenneker.Cloudflare.Middlewares.Require.Functions.Abstract;
using Soenneker.Cloudflare.Validators.Request.Functions.Registrars;

namespace Soenneker.Cloudflare.Middlewares.Require.Functions.Registrars;

/// <summary>
/// A middleware component for Azure Functions that enforces Cloudflare-only access, blocking non-Cloudflare requests unless running in local/test environments.
/// </summary>
public static class RequireCloudflareMiddlewareRegistrar
{
    public static IFunctionsWorkerApplicationBuilder UseRequireCloudflare(this IFunctionsWorkerApplicationBuilder builder)
    {
        builder.UseMiddleware<RequireCloudflareMiddleware>();
        return builder;
    }

    /// <summary>
    /// Adds <see cref="IRequireCloudflareMiddleware"/> as a singleton service. <para/>
    /// </summary>
    public static IServiceCollection AddRequireCloudflareMiddlewareAsSingleton(this IServiceCollection services)
    {
        services.AddCloudflareRequestValidatorAsSingleton();

        return services;
    }

    /// <summary>
    /// Adds <see cref="IRequireCloudflareMiddleware"/> as a scoped service. <para/>
    /// </summary>
    public static IServiceCollection AddRequireCloudflareMiddlewareAsScoped(this IServiceCollection services)
    {
        services.AddCloudflareRequestValidatorAsSingleton();

        return services;
    }
}