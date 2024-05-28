using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Tgs.Authorization.Clients;
using Valhalla.Authorization;

namespace AuthorizationClients.IntegrationTests;

internal class DemoIdentityResolver(IHttpContextAccessor httpContextAccessor) : IAuthorizationIdentityResolveProvider
{
    public IAsyncEnumerable<IAuthorizationIdentity> GetIdentitiesAsync(CancellationToken cancellationToken = default)
        => FindTgsAccounts().ToAsyncEnumerable();

    private IEnumerable<TgsAccount> FindTgsAccounts()
    {
        var httpContext = httpContextAccessor.HttpContext;

        if (httpContext?.User?.Identity is null
            || !httpContext.User.Identity.IsAuthenticated)
            yield break;

        var accountIdText = httpContext.User.FindFirstValue("account_id");

        if (Guid.TryParse(accountIdText, out var accountId))
            yield return new TgsAccount(accountId);
    }
}
