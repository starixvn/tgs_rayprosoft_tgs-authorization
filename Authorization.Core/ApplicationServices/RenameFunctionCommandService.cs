using Tgs.ServiceUtility;

namespace Tgs.Authorization.ApplicationServices;

internal class RenameFunctionCommandService(ITgsRepository tgsRepository)
	: ICommandService<RenameFunctionCommand>
{
	public ValueTask ExecuteAsync(RenameFunctionCommand command, CancellationToken cancellationToken = default)
		=> tgsRepository.RenameFunctionAsync(
			command.FunctionId,
			command.NewName,
			cancellationToken);
}
