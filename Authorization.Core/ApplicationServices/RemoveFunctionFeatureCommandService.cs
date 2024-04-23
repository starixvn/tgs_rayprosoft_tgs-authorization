using Tgs.ServiceUtility;

namespace Tgs.Authorization.ApplicationServices;

internal class RemoveFunctionFeatureCommandService(ITgsRepository tgsRepository)
	: ICommandService<RemoveFunctionFeatureCommand>
{
	public ValueTask ExecuteAsync(RemoveFunctionFeatureCommand command, CancellationToken cancellationToken = default)
		=> tgsRepository.DeleteFunctionFeatureAsync(command.FeatureId, cancellationToken);
}
