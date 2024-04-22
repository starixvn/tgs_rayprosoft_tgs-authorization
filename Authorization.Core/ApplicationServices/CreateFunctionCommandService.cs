using Tgs.ServiceUtility;

namespace Tgs.Authorization.ApplicationServices;

internal class CreateFunctionCommandService(ITgsRepository tgsRepository)
	: IGetService<CreateFunctionCommand, Guid>
{
	public async ValueTask<Guid> GetAsync(CreateFunctionCommand command, CancellationToken cancellationToken = default)
	{
		var newFunctionId = Guid.NewGuid();
		var newFunction = new SystemFunction(newFunctionId, command.FunctionName, command.System);

		await tgsRepository.CreateFunctionAsync(
			newFunction,
			cancellationToken).ConfigureAwait(false);

		return newFunctionId;
	}
}
