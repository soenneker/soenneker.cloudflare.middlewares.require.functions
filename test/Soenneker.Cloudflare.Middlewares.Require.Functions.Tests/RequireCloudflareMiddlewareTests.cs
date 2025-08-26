using Soenneker.Tests.FixturedUnit;
using Xunit;

namespace Soenneker.Cloudflare.Middlewares.Require.Functions.Tests;

[Collection("Collection")]
public sealed class RequireCloudflareMiddlewareTests : FixturedUnitTest
{
    public RequireCloudflareMiddlewareTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
    }

    [Fact]
    public void Default()
    {

    }
}
