using System.Text.Json;
using Tgs.ServiceUtility;

namespace Tgs.Authorization.ApplicationServices;

internal class ListFunctionFeaturesQueryService(ITgsRepository tgsRepository)
	: IQueryService<ListFunctionFeaturesQuery, SystemFunctionFeature<JsonDocument>>
{
	public IAsyncEnumerable<SystemFunctionFeature<JsonDocument>> QueryAsync(ListFunctionFeaturesQuery query, CancellationToken cancellationToken = default)
		=> tgsRepository.GetSystemFunctionFeaturesAsync(query.FunctionId, cancellationToken);
}