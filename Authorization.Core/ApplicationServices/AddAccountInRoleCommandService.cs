using Tgs.ServiceUtility;

namespace Tgs.Authorization.ApplicationServices;

internal class AddAccountInRoleCommandService(ITgsRepository tgsRepository)
	: ICommandService<AddAccountInRoleCommand>
{
	public ValueTask ExecuteAsync(AddAccountInRoleCommand command, CancellationToken cancellationToken = default)
		=> tgsRepository.UpsertAccountInRoleAsync(
			command.AccountId,
			command.RoleId,
			command.Priority,
			cancellationToken);
}