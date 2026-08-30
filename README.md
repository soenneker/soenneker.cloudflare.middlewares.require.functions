[![](https://img.shields.io/nuget/v/soenneker.cloudflare.middlewares.require.functions.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.cloudflare.middlewares.require.functions/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.cloudflare.middlewares.require.functions/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.cloudflare.middlewares.require.functions/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.cloudflare.middlewares.require.functions.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.cloudflare.middlewares.require.functions/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.cloudflare.middlewares.require.functions/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.cloudflare.middlewares.require.functions/actions/workflows/codeql.yml)

# Soenneker.Cloudflare.Middlewares.Require.Functions

Azure Functions isolated-worker middleware that rejects HTTP requests without a recognized Cloudflare client certificate.

## Installation

```bash
dotnet add package Soenneker.Cloudflare.Middlewares.Require.Functions
```

## Registration

```csharp
using Soenneker.Cloudflare.Middlewares.Require.Functions.Registrars;

builder.Services.AddRequireCloudflareMiddlewareAsSingleton();

builder.ConfigureFunctionsWebApplication(application =>
{
    application.UseRequireCloudflare();
});
```

Non-HTTP triggers pass through unchanged. HTTP requests that fail validation receive `403 Forbidden` and do not invoke the function.

## Environment behavior

The middleware is bypassed when the `Environment` configuration value is exactly `Local` or `Test`. It remains enforced for values such as `Development`, `Staging`, and `Production`. Treat this setting as security-sensitive configuration.

## Azure and Cloudflare requirements

The validator reads Azure App Service's `X-ARR-ClientCert` header and compares the forwarded certificate fingerprint with the packaged Cloudflare origin-certificate fingerprints. Configure Cloudflare Authenticated Origin Pulls and enable client-certificate forwarding on the Azure hosting path.

This check is only trustworthy when Azure or another trusted front end removes caller-supplied `X-ARR-ClientCert` values and sets the header from the TLS client certificate. Do not expose a path where an external caller can inject that header directly. Restrict direct origin access with platform networking, Cloudflare Tunnel, firewall rules, or equivalent controls.

`Cloudflare:RequestValidatorLog` enables validator diagnostics. Avoid verbose security logging unless it is needed for troubleshooting.
