using Microsoft.AspNetCore.Mvc;
using Tgs.Authorization.ApplicationServices;
using Tgs.Authorization.ViewModels;
using Tgs.ServiceUtility;

namespace Tgs.Authorization.Controllers;

[Route("[controller]")]
[ApiController]
public class GrantController : ControllerBase
{
	[HttpPut("account/{accountId:Guid}", Name = "AssignAccountGrant")]
	public ValueTask AssignAccountGrantAsync(
		[FromServices] ICommandService<AssignAccountGrantCommand> assignService,
		Guid accountId,
		[FromBody] GrantRequestViewModel viewModel)
	{
		var command = new AssignAccountGrantCommand(
			accountId,
			viewModel.FunctionId,
			viewModel.Type);

		return assignService.ExecuteAsync(command, HttpContext.RequestAborted);
	}

	[HttpPut("role/{roleId:Guid}", Name = "AssignRoleGrant")]
	public ValueTask AssignRoleGrantAsync(
		[FromServices] ICommandService<AssignRoleGrantCommand> assignService,
		Guid roleId,
		GrantRequestViewModel viewModel)
	{
		var command = new AssignRoleGrantCommand(
			roleId,
			viewModel.FunctionId,
			viewModel.Type);

		return assignService.ExecuteAsync(command, HttpContext.RequestAborted);
	}

	[HttpDelete("account/{accountId:Guid}/{functionId:Guid}", Name = "RemoveAccountGrant")]
	public ValueTask RemoveAccountGrantAsync(
		[FromServices] ICommandService<RemoveAccountGrantCommand> removeService,
		Guid accountId,
		Guid functionId)
	{
		var command = new RemoveAccountGrantCommand(accountId, functionId);

		return removeService.ExecuteAsync(command, HttpContext.RequestAborted);
	}

	[HttpDelete("role/{roleId:Guid}/{functionId:Guid}", Name = "RemoveRoleGrant")]
	public ValueTask RemoveRoleGrantAsync(
		[FromServices] ICommandService<RemoveRoleGrantCommand> removeService,
		Guid roleId,
		Guid functionId)
	{
		var command = new RemoveRoleGrantCommand(roleId, functionId);

		return removeService.ExecuteAsync(command, HttpContext.RequestAborted);
	}
}