using Soenneker.Tests.HostedUnit;

namespace Soenneker.Cloudflare.Middlewares.Require.Functions.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public sealed class RequireCloudflareMiddlewareTests : HostedUnitTest
{
    public RequireCloudflareMiddlewareTests(Host host) : base(host)
    {
    }

    [Test]
    public void Default()
    {

    }
}
