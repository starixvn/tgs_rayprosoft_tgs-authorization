using Tgs.ServiceUtility;

namespace Tgs.Authorization.ApplicationServices;
internal class DeleteFunctionCommandService(ITgsRepository tgsRepository)
	: ICommandService<DeleteFunctionCommand>
{
	public ValueTask ExecuteAsync(DeleteFunctionCommand command, CancellationToken cancellationToken = default)
		=> tgsRepository.DeleteFunctionAsync(command.FunctionId, cancellationToken);
}
