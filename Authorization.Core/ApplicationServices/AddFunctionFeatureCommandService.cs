using System.Text.Json;
using Tgs.ServiceUtility;

namespace Tgs.Authorization.ApplicationServices;

internal class AddFunctionFeatureCommandService(ITgsRepository tgsRepository)
	: IGetService<AddFunctionFeatureCommand, Guid>
{
	public async ValueTask<Guid> GetAsync(AddFunctionFeatureCommand command, CancellationToken cancellationToken = default)
	{
		var newFeatureId = Guid.NewGuid();
		var newFeature = new SystemFunctionFeature<string>(
			newFeatureId,
			command.FeatureName,
			command.FeatureType,
			JsonSerializer.Serialize(command.FeatureSettings, AuthorizationUtility.SerializerOptions));

		await tgsRepository.InsertFunctionFeatureAsync(
			command.FunctionId,
			newFeature,
			cancellationToken).ConfigureAwait(false);

		return newFeatureId;
	}
}
