using Tgs.ServiceUtility;

namespace Tgs.Authorization.ApplicationServices;

internal class ListAccountGrantsQueryService(ITgsRepository tgsRepository)
	: IQueryService<ListAccountGrantsQuery, AccountGrant>
{
	public IAsyncEnumerable<AccountGrant> QueryAsync(ListAccountGrantsQuery query, CancellationToken cancellationToken = default)
		=> tgsRepository.GetAccountFunctionGrantsAsync(query.AccountId, cancellationToken);
}