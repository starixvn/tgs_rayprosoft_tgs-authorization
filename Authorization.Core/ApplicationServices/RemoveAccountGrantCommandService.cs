using Tgs.ServiceUtility;

namespace Tgs.Authorization.ApplicationServices;

internal class RemoveAccountGrantCommandService(ITgsRepository tgsRepository)
	: ICommandService<RemoveAccountGrantCommand>
{
	public ValueTask ExecuteAsync(RemoveAccountGrantCommand command, CancellationToken cancellationToken = default)
		=> tgsRepository.DeleteAccountFunctionGrantAsync(
			command.FunctionId,
			command.AccountId,
			cancellationToken);
}
