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
    private readonly bool _exclude;

    public RequireCloudflareMiddleware(ILogger<RequireCloudflareMiddleware> logger, ICloudflareRequestValidator validator, IConfiguration config)
    {
        _logger = logger;
        _validator = validator;

        var environment = config.GetValueStrict<string>("Environment");

        if (environment == DeployEnvironment.Local || environment == DeployEnvironment.Test)
            _exclude = true;
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

        if (_exclude)
        {
            await next(context);
            return;
        }

        bool isCloudflare = await _validator.IsFromCloudflare(req).NoSync();

        if (!isCloudflare)
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