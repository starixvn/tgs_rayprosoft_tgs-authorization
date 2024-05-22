using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AuthorizationClients.IntegrationTests;
public class ClientTests
{
    [Fact]
    public async Task 整合測試_授權Api呼叫_沒有權限會得到403回應()
    {
        // Arrange
        var system = "Test";

        var webBuilder = WebApplication.CreateBuilder();

        _ = webBuilder.Services
            .AddPhoenixAuthorizationKits(system)
            .Services
            .AddWebApiAuthorizationMiddlewareResultHandler()
            .AddAuthorizationBuilder()
            .AddHttpFallbackPolicy();

        _ = webBuilder.WebHost.UseTestServer();

        await using var web = webBuilder.Build();

        web.UseCaptureFunction();
        _ = web.MapGet("/info", () => "test");

        await web.StartAsync();

        var http = web.GetTestClient();

        // Act
        var response = await http.GetAsync("/info");

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }
}
