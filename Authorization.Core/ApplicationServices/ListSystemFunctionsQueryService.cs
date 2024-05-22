using Tgs.ServiceUtility;

namespace Tgs.Authorization.ApplicationServices;

internal class ListSystemFunctionsQueryService(ITgsRepository tgsRepository)
	: IQueryService<ListSystemFunctionsQuery, SystemFunction>
{
	public IAsyncEnumerable<SystemFunction> QueryAsync(ListSystemFunctionsQuery query, CancellationToken cancellationToken = default)
		=> tgsRepository.GetSystemFunctionsAsync(query.System, cancellationToken);
}