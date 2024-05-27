using Tgs.ServiceUtility;

namespace Tgs.Authorization.ApplicationServices;

internal class AssignRoleGrantCommandService(ITgsRepository tgsRepository)
	: ICommandService<AssignRoleGrantCommand>
{
	public ValueTask ExecuteAsync(AssignRoleGrantCommand command, CancellationToken cancellationToken = default)
		=> tgsRepository.UpsertRoleFunctionGrantAsync(
			new RoleGrant(
				command.FunctionId,
				command.RoleId,
				command.Type),
			cancellationToken);
}