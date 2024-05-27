using Microsoft.AspNetCore.Authentication;

namespace AuthorizationClients.IntegrationTests;

internal class StubAuthenticationOptions : AuthenticationSchemeOptions
{
    public AuthenticationTicket? LoginTicket { get; set; }
}