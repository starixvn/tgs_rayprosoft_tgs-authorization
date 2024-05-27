using Microsoft.AspNetCore.Mvc;
using Tgs.Authorization.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.Authorization.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthorizeController : ControllerBase
{
	[HttpGet("{accountId:Guid}/{functionId:Guid}", Name = "Authorize")]
	public ValueTask<bool> AuthorizeAsync(
		[FromServices] IGetService<CheckAccountHasPermissionCommand, bool> checkService,
		Guid accountId,
		Guid functionId)
	{
		var command = new CheckAccountHasPermissionCommand(accountId, functionId);

		return checkService.GetAsync(command, HttpContext.RequestAborted);
	}
}