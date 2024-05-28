using Tgs.ServiceUtility;

namespace Tgs.Authorization.ApplicationServices;

internal class AddRoleInRoleCommandService(ITgsRepository tgsRepository)
	: ICommandService<AddRoleInRoleCommand>
{
	public ValueTask ExecuteAsync(AddRoleInRoleCommand command, CancellationToken cancellationToken = default)
		=> tgsRepository.UpsertRoleInRoleAsync(
			command.RoleId,
			command.ParentRoleId,
			command.Priority,
			cancellationToken);
}
