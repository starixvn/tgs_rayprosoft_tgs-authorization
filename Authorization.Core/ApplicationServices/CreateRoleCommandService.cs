using Tgs.ServiceUtility;

namespace Tgs.Authorization.ApplicationServices;

internal class CreateRoleCommandService(
	ITgsRepository tgsRepository)
	: IGetService<CreateRoleCommand, Guid>
{
	public async ValueTask<Guid> GetAsync(CreateRoleCommand command, CancellationToken cancellationToken = default)
	{
		var newId = Guid.NewGuid();
		var newRole = new Role(newId, command.RoleName, command.System);

		await tgsRepository.CreateRoleAsync(
			newRole,
			cancellationToken).ConfigureAwait(false);

		return newId;
	}
}
