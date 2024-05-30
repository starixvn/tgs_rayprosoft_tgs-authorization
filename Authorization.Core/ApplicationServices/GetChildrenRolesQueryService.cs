using Tgs.ServiceUtility;

namespace Tgs.Authorization.ApplicationServices;

internal class GetChildrenRolesQueryService(ITgsRepository tgsRepository) : IQueryService<GetChildrenRolesQuery, Guid>
{
	public IAsyncEnumerable<Guid> QueryAsync(GetChildrenRolesQuery query, CancellationToken cancellationToken = default)
		=> tgsRepository.GetChildrenRolesAsync(query.RoleId, cancellationToken);
}