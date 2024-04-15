using Tgs.ServiceUtility;

namespace Tgs.Authorization.ApplicationServices;

internal class GetSystemAllRolesQueryService(ITgsRepository tgsRepository)
	: IQueryService<GetSystemAllRolesQuery, Role>
{
	public IAsyncEnumerable<Role> QueryAsync(GetSystemAllRolesQuery query, CancellationToken cancellationToken = default)
		=> tgsRepository.GetSystemAllRolesAsync(query.System, cancellationToken);
}
