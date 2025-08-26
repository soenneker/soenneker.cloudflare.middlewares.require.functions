using Microsoft.Azure.Functions.Worker.Middleware;

namespace Soenneker.Cloudflare.Middlewares.Require.Functions.Abstract;

/// <summary>
/// A middleware component for Azure Functions that enforces Cloudflare-only access, blocking non-Cloudflare requests unless running in local/test environments.
/// </summary>
public interface IRequireCloudflareMiddleware : IFunctionsWorkerMiddleware
{
}
