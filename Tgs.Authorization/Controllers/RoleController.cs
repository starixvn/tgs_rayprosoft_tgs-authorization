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

	[HttpPut("{roleId:Guid}", Name = "DeleteRole")]
	public ValueTask DeleteRoleAsync(
		[FromServices] ICommandService<DeleteRoleCommand> deleteService,
		Guid roleId)
	{
		var command = new DeleteRoleCommand(roleId);

		return deleteService.ExecuteAsync(command, HttpContext.RequestAborted);
	}
}
