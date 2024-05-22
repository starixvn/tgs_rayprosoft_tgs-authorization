using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valhalla.Authorization;

namespace Tgs.Authorization.Clients;
internal class AuthorizationDataStore : IAuthorizationDataStore
{
	public IAsyncEnumerable<IAuthorizationFunction> GetFunctionsAsync(string system, CancellationToken cancellationToken = default) => throw new NotImplementedException();
	public ValueTask<IAuthorizationFunction?> FindFunctionAsync(string system, string functionName, CancellationToken cancellationToken = default) => throw new NotImplementedException();
	public ValueTask<bool> CheckHasPermissionAsync(string system, IAuthorizationFunction function, IEnumerable<IAuthorizationIdentity> identities, CancellationToken cancellationToken = default) => throw new NotImplementedException();
}
