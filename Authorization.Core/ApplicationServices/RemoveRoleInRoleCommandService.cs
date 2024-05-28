using Tgs.ServiceUtility;

namespace Tgs.Authorization.ApplicationServices;

internal class RemoveRoleInRoleCommandService(ITgsRepository tgsRepository)
	: ICommandService<RemoveRoleInRoleCommand>
{
	public ValueTask ExecuteAsync(RemoveRoleInRoleCommand command, CancellationToken cancellationToken = default)
		=> tgsRepository.RemoveRoleInRoleAsync(
			command.RoleId,
			command.ParentRoleId,
			cancellationToken);
}
