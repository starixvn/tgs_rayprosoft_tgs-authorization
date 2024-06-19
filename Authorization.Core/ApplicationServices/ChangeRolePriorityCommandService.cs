using Tgs.ServiceUtility;

namespace Tgs.Authorization.ApplicationServices;

internal class ChangeRolePriorityCommandService(ITgsRepository tgsRepository)
	: ICommandService<ChangeRolePriorityCommand>
{
	public ValueTask ExecuteAsync(ChangeRolePriorityCommand command, CancellationToken cancellationToken = default)
		=> tgsRepository.SetRolePriorityAsync(command.RoleId, command.NewPriority, cancellationToken);
}
