using Tgs.ServiceUtility;

namespace Tgs.Authorization.ApplicationServices;

internal class DeleteRoleCommandService(ITgsRepository tgsRepository)
	: ICommandService<DeleteRoleCommand>
{
	public ValueTask ExecuteAsync(DeleteRoleCommand command, CancellationToken cancellationToken = default)
		=> tgsRepository.DeleteRoleAsync(command.RoleId, cancellationToken);
}
