using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Soenneker.Cloudflare.Middlewares.Require.Functions.Abstract;
using Soenneker.Cloudflare.Validators.Request.Functions.Abstract;
using Soenneker.Enums.DeployEnvironment;
using Soenneker.Extensions.Configuration;
using Soenneker.Extensions.Task;
using Soenneker.Extensions.ValueTask;

namespace Soenneker.Cloudflare.Middlewares.Require.Functions;

///<inheritdoc cref="IRequireCloudflareMiddleware"/>
public sealed class RequireCloudflareMiddleware : IRequireCloudflareMiddleware
{
    private readonly ILogger<RequireCloudflareMiddleware> _logger;
    private readonly ICloudflareRequestValidator _validator;
    private readonly IConfiguration _config;

    public RequireCloudflareMiddleware(ILogger<RequireCloudflareMiddleware> logger, ICloudflareRequestValidator validator, IConfiguration config)
    {
        _logger = logger;
        _validator = validator;
        _config = config;
    }

    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        // Only applies to HTTP triggers
        HttpRequestData? req = await context.GetHttpRequestDataAsync().NoSync();

        if (req is null)
        {
            await next(context).NoSync();
            return;
        }

        var environment = _config.GetValueStrict<string>("Environment");
        if (environment == DeployEnvironment.Local || environment == DeployEnvironment.Test)
        {
            await next(context);
            return;
        }

        bool ok = await _validator.IsFromCloudflare(req).NoSync();

        if (!ok)
        {
            HttpResponseData res = req.CreateResponse(HttpStatusCode.Forbidden);
            await res.WriteStringAsync("Forbidden").NoSync();
            context.GetInvocationResult().Value = res;
            _logger.LogWarning("Blocked non-Cloudflare request from {Ip}", req?.Url?.Host);
            return;
        }

        await next(context).NoSync();
    }
}