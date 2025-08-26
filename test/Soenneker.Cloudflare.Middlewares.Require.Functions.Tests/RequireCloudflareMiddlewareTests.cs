using Soenneker.Cloudflare.Middlewares.Require.Functions.Abstract;
using Soenneker.Tests.FixturedUnit;
using Xunit;

namespace Soenneker.Cloudflare.Middlewares.Require.Functions.Tests;

[Collection("Collection")]
public sealed class RequireCloudflareMiddlewareTests : FixturedUnitTest
{
    private readonly IRequireCloudflareMiddleware _util;

    public RequireCloudflareMiddlewareTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _util = Resolve<IRequireCloudflareMiddleware>(true);
    }

    [Fact]
    public void Default()
    {

    }
}
