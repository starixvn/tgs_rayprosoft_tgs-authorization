using Tgs.ServiceUtility;

namespace Tgs.Authorization.ApplicationServices;

internal class GetChildrenRolesQueryService(ITgsRepository tgsRepository)
	: IQueryService<GetChildrenRolesQuery, Role>
{
	public IAsyncEnumerable<Role> QueryAsync(GetChildrenRolesQuery query, CancellationToken cancellationToken = default)
		=> tgsRepository.GetChildrenRolesAsync(query.RoleId, cancellationToken);
}