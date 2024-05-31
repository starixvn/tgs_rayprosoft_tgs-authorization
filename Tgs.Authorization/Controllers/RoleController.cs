using Microsoft.AspNetCore.Mvc;
using Tgs.Authorization.ApplicationServices;
using Tgs.Authorization.ViewModels;
using Tgs.ServiceUtility;

namespace Tgs.Authorization.Controllers;

[Route("[controller]")]
[ApiController]
public class RoleController : ControllerBase
{
	[HttpGet("{system}", Name = "GetRolesBySystem")]
	public IAsyncEnumerable<RoleViewModel> GetRolesBySystemAsync(
		[FromServices] IQueryService<GetSystemAllRolesQuery, Role> queryService,
		string system)
	{
		var query = new GetSystemAllRolesQuery(system);

		return queryService.QueryAsync(query, HttpContext.RequestAborted)
			.Select(role => new RoleViewModel
			{
				Id = role.Id,
				Name = role.Name
			});
	}

	[HttpPut("{system}", Name = "CreateRole")]
	public ValueTask<Guid> CreateRoleAsync(
		[FromServices] IGetService<CreateRoleCommand, Guid> createRoleService,
		string system,
		[FromBody] CreateRoleViewModel viewModel)
	{
		var command = new CreateRoleCommand(system, viewModel.Name);

		return createRoleService.GetAsync(command, HttpContext.RequestAborted);
	}

	[HttpDelete("{roleId:Guid}", Name = "DeleteRole")]
	public ValueTask DeleteRoleAsync(
		[FromServices] ICommandService<DeleteRoleCommand> deleteService,
		Guid roleId)
	{
		var command = new DeleteRoleCommand(roleId);

		return deleteService.ExecuteAsync(command, HttpContext.RequestAborted);
	}

	[HttpPut("account/{accountId:Guid}", Name = "AddAcountInRole")]
	public ValueTask AddAccountInRoleAsync(
		[FromServices] ICommandService<AddAccountInRoleCommand> addService,
		Guid accountId,
		[FromBody] AddInRoleViewModel viewModel)
	{
		var command = new AddAccountInRoleCommand(
			accountId,
			viewModel.RoleId,
			viewModel.Priority);

		return addService.ExecuteAsync(command, HttpContext.RequestAborted);
	}

	[HttpDelete("account/{accountId:Guid}/{roleId:Guid}", Name = "RemoveAccountInRole")]
	public ValueTask RemoveAccountInRoleAsync(
		[FromServices] ICommandService<RemoveAccountInRoleCommand> removeService,
		Guid accountId,
		Guid roleId)
	{
		var command = new RemoveAccountInRoleCommand(accountId, roleId);

		return removeService.ExecuteAsync(command, HttpContext.RequestAborted);
	}

	[HttpGet("account/{roleId:Guid}", Name = "ListRoleAccounts")]
	public IAsyncEnumerable<Guid> ListRoleAccountsAsync(
		[FromServices] IQueryService<ListRoleAccountsQuery, Guid> queryService,
		Guid roleId)
	{
		var query = new ListRoleAccountsQuery(roleId);

		return queryService.QueryAsync(query, HttpContext.RequestAborted);
	}

	[HttpPut("in-role/{roleId:Guid}", Name = "AddRoleInRole")]
	public ValueTask AddRoleInRoleAsync(
		[FromServices] ICommandService<AddRoleInRoleCommand> addService,
		Guid roleId,
		[FromBody] AddInRoleViewModel viewModel)
	{
		var command = new AddRoleInRoleCommand(roleId, viewModel.RoleId, viewModel.Priority);

		return addService.ExecuteAsync(command, HttpContext.RequestAborted);
	}

	[HttpDelete("in-role/{roleId:Guid}/{parentRoleId:Guid}", Name = "RemoveRoleInRole")]
	public ValueTask RemoveRoleInRoleAsync(
		[FromServices] ICommandService<RemoveRoleInRoleCommand> removeService,
		Guid roleId,
		Guid parentRoleId)
	{
		var command = new RemoveRoleInRoleCommand(roleId, parentRoleId);

		return removeService.ExecuteAsync(command, HttpContext.RequestAborted);
	}

	[HttpGet("in-role/{parentRoleId:Guid}", Name = "ListChildrenRows")]
	public IAsyncEnumerable<RoleViewModel> ListChildrenRowsAsync(
		[FromServices] IQueryService<GetChildrenRolesQuery, Role> queryService,
		Guid parentRoleId)
	{
		var query = new GetChildrenRolesQuery(parentRoleId);

		return queryService.QueryAsync(query, HttpContext.RequestAborted)
			.Select(role => new RoleViewModel
			{
				Id = role.Id,
				Name = role.Name,
				Priority = role.Priority
			});
	}
}