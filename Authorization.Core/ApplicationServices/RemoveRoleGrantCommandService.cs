using Tgs.ServiceUtility;

namespace Tgs.Authorization.ApplicationServices;

internal class RemoveRoleGrantCommandService(ITgsRepository tgsRepository)
	: ICommandService<RemoveRoleGrantCommand>
{
	public ValueTask ExecuteAsync(RemoveRoleGrantCommand command, CancellationToken cancellationToken = default)
		=> tgsRepository.DeleteRoleFunctionGrantAsync(
			command.FunctionId,
			command.RoleId,
			cancellationToken);
}
