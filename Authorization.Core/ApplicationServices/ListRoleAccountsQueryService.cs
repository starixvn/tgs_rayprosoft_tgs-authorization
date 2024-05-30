using Tgs.ServiceUtility;

namespace Tgs.Authorization.ApplicationServices;

internal class ListRoleAccountsQueryService(ITgsRepository tgsRepository)
	: IQueryService<ListRoleAccountsQuery, Guid>
{
	public IAsyncEnumerable<Guid> QueryAsync(ListRoleAccountsQuery query, CancellationToken cancellationToken = default)
		=> tgsRepository.GetRoleAccountsAsync(query.RoleId, cancellationToken);
}