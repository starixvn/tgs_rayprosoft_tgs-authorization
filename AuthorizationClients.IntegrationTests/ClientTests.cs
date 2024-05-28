using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tgs.Authorization.Clients;
using Tgs.Authorization.Clients.DependencyInjection;
using Valhalla.Authorization;
using Xunit;
using Xunit.Abstractions;

namespace AuthorizationClients.IntegrationTests;

public class ClientTests(ITestOutputHelper testOutputHelper)
{
    [Fact]
    public async Task 整合測試_可以正確識別HttpFunction()
    {
        // Arrange
        var system = "Test";

        var webBuilder = WebApplication.CreateBuilder();

        _ = webBuilder.Logging.AddXUnit(testOutputHelper);

        var fakeAuthorizationClient = Substitute.For<IAuthorizationClient>();

        _ = webBuilder.Services
            .AddAuthentication()
            .AddBearerToken()
            .Services
            .AddSingleton(fakeAuthorizationClient)
            .AddPhoenixAuthorizationKits(system)
            .RegisterTgsAuthorizationDataStore()
            .RegisterIdentityResolveProvider<DemoIdentityResolver>()
            .Services
            .AddWebApiAuthorizationMiddlewareResultHandler()
            .AddAuthorizationBuilder()
            .AddHttpFallbackPolicy();

        _ = webBuilder.WebHost.UseTestServer();

        await using var web = webBuilder.Build();

        web.UseCaptureFunction();
        _ = web.MapGet("/info", (HttpContext context) => context.Features.Get<IAuthorizationFunction>()?.Id)
            .AllowAnonymous();

        var functionId = Guid.NewGuid();

        await web.StartAsync();

        var http = web.GetTestClient();

        _ = fakeAuthorizationClient.ListSystemFunctionsAsync(Arg.Is(system), Arg.Any<CancellationToken>())
            .Returns(new[]
            {
                new SystemFunction(functionId, "func1", system)
            }.ToAsyncEnumerable());

        _ = fakeAuthorizationClient.ListSystemFunctionFeaturesAsync(Arg.Is(functionId), Arg.Any<CancellationToken>())
            .Returns(new[]
            {
                new SystemFunctionFeature(
                    Guid.NewGuid(),
                    "feat1",
                    FeatureType.Http,
                    JsonSerializer.SerializeToDocument(
                        new HttpFeatureSettings(["GET"], "/info"),
                        ClientUtility.SerializerOptions))
            }.ToAsyncEnumerable());

        // Act
        var actual = await http.GetFromJsonAsync<Guid?>("/info");

        // Assert
        Assert.Equal(functionId, actual);
    }

    [Fact]
    public async Task 整合測試_授權Api呼叫_沒有權限會得到403回應()
    {
        // Arrange
        var system = "Test";
        var accountId = Guid.NewGuid();

        var webBuilder = WebApplication.CreateBuilder();

        var fakeAuthorizationClient = Substitute.For<IAuthorizationClient>();

        _ = webBuilder.Services
            .AddAuthentication()
            .AddScheme<StubAuthenticationOptions, StubAuthenticationHandler>(
                StubAuthenticationDefaults.AuthenticationScheme,
                options => options.LoginTicket = new AuthenticationTicket(
                    new ClaimsPrincipal(
                        new ClaimsIdentity([
                            new Claim(ClaimTypes.NameIdentifier, "test:test1"),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim("account_id", accountId.ToString()),
                            new Claim("session_id", Guid.NewGuid().ToString()),
                            new Claim("system", system)
                        ])),
                    StubAuthenticationDefaults.AuthenticationScheme))
            .Services
            .AddHttpContextAccessor()
            .AddSingleton(fakeAuthorizationClient)
            .AddPhoenixAuthorizationKits(system)
            .RegisterTgsAuthorizationDataStore()
            .RegisterIdentityResolveProvider<DemoIdentityResolver>()
            .Services
            .AddWebApiAuthorizationMiddlewareResultHandler()
            .AddAuthorizationBuilder()
            .AddHttpFallbackPolicy();

        _ = webBuilder.WebHost.UseTestServer();

        await using var web = webBuilder.Build();

        web.UseCaptureFunction();
        web.UseAuthorization();
        _ = web.MapGet("/info", () => "test");

        var functionId = Guid.NewGuid();

        await web.StartAsync();

        var http = web.GetTestClient();

        _ = fakeAuthorizationClient.ListSystemFunctionsAsync(Arg.Is(system), Arg.Any<CancellationToken>())
            .Returns(new[]
            {
                    new SystemFunction(functionId, "func1", system)
            }.ToAsyncEnumerable());

        _ = fakeAuthorizationClient.ListSystemFunctionFeaturesAsync(Arg.Is(functionId), Arg.Any<CancellationToken>())
            .Returns(new[]
            {
                new SystemFunctionFeature(
                    Guid.NewGuid(),
                    "feat1",
                    FeatureType.Http,
                    JsonSerializer.SerializeToDocument(
                        new HttpFeatureSettings(["GET"], "/info"),
                        ClientUtility.SerializerOptions))
            }.ToAsyncEnumerable());

        // Act
        var response = await http.GetAsync("/info");

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task 整合測試_授權Api呼叫_沒有登入會得到401回應()
    {
        // Arrange
        var system = "Test";
        var accountId = Guid.NewGuid();

        var webBuilder = WebApplication.CreateBuilder();

        var fakeAuthorizationClient = Substitute.For<IAuthorizationClient>();

        _ = webBuilder.Services
            .AddAuthentication()
            .AddScheme<StubAuthenticationOptions, StubAuthenticationHandler>(
                StubAuthenticationDefaults.AuthenticationScheme,
                options => options.LoginTicket = null)
            .Services
            .AddHttpContextAccessor()
            .AddSingleton(fakeAuthorizationClient)
            .AddPhoenixAuthorizationKits(system)
            .RegisterTgsAuthorizationDataStore()
            .RegisterIdentityResolveProvider<DemoIdentityResolver>()
            .Services
            .AddWebApiAuthorizationMiddlewareResultHandler()
            .AddAuthorizationBuilder()
            .AddHttpFallbackPolicy();

        _ = webBuilder.WebHost.UseTestServer();

        await using var web = webBuilder.Build();

        web.UseCaptureFunction();
        web.UseAuthorization();
        _ = web.MapGet("/info", () => "test");

        var functionId = Guid.NewGuid();

        await web.StartAsync();

        var http = web.GetTestClient();

        _ = fakeAuthorizationClient.ListSystemFunctionsAsync(Arg.Is(system), Arg.Any<CancellationToken>())
            .Returns(new[]
            {
                new SystemFunction(functionId, "func1", system)
            }.ToAsyncEnumerable());

        _ = fakeAuthorizationClient.ListSystemFunctionFeaturesAsync(Arg.Is(functionId), Arg.Any<CancellationToken>())
            .Returns(new[]
            {
                new SystemFunctionFeature(
                    Guid.NewGuid(),
                    "feat1",
                    FeatureType.Http,
                    JsonSerializer.SerializeToDocument(
                        new HttpFeatureSettings(["GET"], "/info"),
                        ClientUtility.SerializerOptions))
            }.ToAsyncEnumerable());

        // Act
        var response = await http.GetAsync("/info");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task 整合測試_Api呼叫_授權通過()
    {
        // Arrange
        var system = "Test";
        var accountId = Guid.NewGuid();

        var webBuilder = WebApplication.CreateBuilder();

        _ = webBuilder.Configuration
            .AddInMemoryCollection(
                new Dictionary<string, string?>
                {
                    ["ENVIRONMENT"] = "Development"
                });

        _ = webBuilder.Logging
            .ClearProviders()
            .AddXUnit(testOutputHelper);

        var fakeAuthorizationClient = Substitute.For<IAuthorizationClient>();

        _ = webBuilder.Services
            .AddAuthentication()
            .AddScheme<StubAuthenticationOptions, StubAuthenticationHandler>(
                StubAuthenticationDefaults.AuthenticationScheme,
                options => options.LoginTicket = new AuthenticationTicket(
                    new ClaimsPrincipal(
                        new ClaimsIdentity(
                            [
                                new Claim(ClaimTypes.NameIdentifier, "test:test1"),
                                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                                new Claim("account_id", accountId.ToString()),
                                new Claim("session_id", Guid.NewGuid().ToString()),
                                new Claim("system", system)
                            ],
                            StubAuthenticationDefaults.AuthenticationScheme)),
                    StubAuthenticationDefaults.AuthenticationScheme))
            .Services
            .AddHttpContextAccessor()
            .AddSingleton(fakeAuthorizationClient)
            .AddPhoenixAuthorizationKits(system)
            .RegisterTgsAuthorizationDataStore()
            .RegisterIdentityResolveProvider<DemoIdentityResolver>()
            .Services
            .AddWebApiAuthorizationMiddlewareResultHandler()
            .AddAuthorizationBuilder()
            .AddHttpFallbackPolicy();

        _ = webBuilder.WebHost.UseTestServer();

        await using var web = webBuilder.Build();

        web.UseCaptureFunction();
        web.UseAuthorization();
        _ = web.MapGet("/info", () => "test");

        var functionId = Guid.NewGuid();

        await web.StartAsync();

        var http = web.GetTestClient();

        _ = fakeAuthorizationClient.ListSystemFunctionsAsync(Arg.Is(system), Arg.Any<CancellationToken>())
            .Returns(new[]
            {
                new SystemFunction(functionId, "func1", system)
            }.ToAsyncEnumerable());

        _ = fakeAuthorizationClient.ListSystemFunctionFeaturesAsync(Arg.Is(functionId), Arg.Any<CancellationToken>())
            .Returns(new[]
            {
                new SystemFunctionFeature(
                    Guid.NewGuid(),
                    "feat1",
                    FeatureType.Http,
                    JsonSerializer.SerializeToDocument(
                        new HttpFeatureSettings(["GET"], "/info"),
                        ClientUtility.SerializerOptions))
            }.ToAsyncEnumerable());

        _ = fakeAuthorizationClient.AuthorizeAsync(
            Arg.Is(accountId),
            Arg.Is(functionId),
            Arg.Any<CancellationToken>())
            .Returns(true);

        // Act
        var response = await http.GetAsync("/info");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
