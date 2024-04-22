using Tgs.ServiceUtility;

namespace Tgs.Authorization.ApplicationServices;

internal class AssignAccountGrantCommandService(ITgsRepository tgsRepository)
	: ICommandService<AssignAccountGrantCommand>
{
	public ValueTask ExecuteAsync(AssignAccountGrantCommand command, CancellationToken cancellationToken = default)
		=> tgsRepository.UpsertAccountFunctionGrantAsync(
			new AccountGrant(
				command.FunctionId,
				command.AccountId,
				command.Type),
			cancellationToken);
}
