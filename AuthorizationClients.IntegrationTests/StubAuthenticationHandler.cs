using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AuthorizationClients.IntegrationTests;

internal class StubAuthenticationHandler(
    IOptionsMonitor<StubAuthenticationOptions> optionsMonitor,
    ILoggerFactory loggerFactory,
    UrlEncoder urlEncoder)
    : AuthenticationHandler<StubAuthenticationOptions>(optionsMonitor, loggerFactory, urlEncoder)
{
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var ticket = Options.LoginTicket;

        return ticket is not null
            ? Task.FromResult(AuthenticateResult.Success(ticket))
            : Task.FromResult(AuthenticateResult.NoResult());
    }
}