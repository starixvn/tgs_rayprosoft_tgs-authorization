using Tgs.ServiceUtility;

namespace Tgs.Authorization.ApplicationServices;

internal class ListRoleGrantsQueryService(ITgsRepository tgsRepository)
	: IQueryService<ListRoleGrantsQuery, RoleGrant>
{
	public IAsyncEnumerable<RoleGrant> QueryAsync(ListRoleGrantsQuery query, CancellationToken cancellationToken = default)
		=> tgsRepository.GetRoleFunctionGrantsAsync(query.RoleId, cancellationToken);
}