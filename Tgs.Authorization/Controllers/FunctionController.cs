using Microsoft.AspNetCore.Mvc;
using Tgs.Authorization.ApplicationServices;
using Tgs.Authorization.ViewModels;
using Tgs.ServiceUtility;

namespace Tgs.Authorization.Controllers;

[Route("[controller]")]
[ApiController]
public class FunctionController : ControllerBase
{
	[HttpPut(Name = "CreateFunction")]
	public ValueTask<Guid> CreateFunctionAsync(
		[FromServices] IGetService<CreateFunctionCommand, Guid> createService,
		CreateFunctionViewModel viewModel)
	{
		var command = new CreateFunctionCommand(viewModel.Name, viewModel.System);

		return createService.GetAsync(command, HttpContext.RequestAborted);
	}

	[HttpPatch("{functionId:Guid}", Name = "RenameFunction")]
	public ValueTask RenameFunctionAsync(
		[FromServices] ICommandService<RenameFunctionCommand> renameService,
		Guid functionId,
		[FromBody] string newName)
	{
		var command = new RenameFunctionCommand(functionId, newName);

		return renameService.ExecuteAsync(command, HttpContext.RequestAborted);
	}

	[HttpDelete("{functionId:guid}", Name = "DeleteFunction")]
	public ValueTask DeleteFunctionAsync(
		[FromServices] ICommandService<DeleteFunctionCommand> deleteService,
		Guid functionId)
	{
		var command = new DeleteFunctionCommand(functionId);

		return deleteService.ExecuteAsync(command, HttpContext.RequestAborted);
	}
}
