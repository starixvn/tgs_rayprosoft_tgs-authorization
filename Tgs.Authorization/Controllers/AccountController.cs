using Microsoft.AspNetCore.Mvc;
using Tgs.Authorization.ApplicationServices;
using Tgs.Authorization.ViewModels;
using Tgs.ServiceUtility;

namespace Tgs.Authorization.Controllers;

[Route("[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
	[HttpGet("roles/{accountId:Guid}", Name = "ListAccountInRoles")]
	public IAsyncEnumerable<RoleViewModel> ListInRolesAsync(
		[FromServices] IQueryService<ListAccountRolesQuery, Role> queryService,
		Guid accountId)
	{
		var query = new ListAccountRolesQuery(accountId);

		return queryService.QueryAsync(query, HttpContext.RequestAborted)
			.Select(role => new RoleViewModel
			{
				Id = role.Id,
				Name = role.Name,
				Priority = role.Priority
			});
	}
}