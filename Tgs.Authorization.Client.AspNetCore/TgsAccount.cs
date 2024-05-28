using Valhalla.Authorization;

namespace Tgs.Authorization.Clients;
public record TgsAccount(Guid Id) : IAuthorizationIdentity;
