using Tgs.ServiceUtility;

namespace Tgs.Authorization.ApplicationServices;

internal class ListAccountRolesQueryService(ITgsRepository tgsRepository)
	: IQueryService<ListAccountRolesQuery, Role>
{
	public IAsyncEnumerable<Role> QueryAsync(ListAccountRolesQuery query, CancellationToken cancellationToken = default)
		=> tgsRepository.GetAccountAllRolesAsync(
			query.AccountId,
			false,
			cancellationToken);
}