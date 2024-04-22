using System.ComponentModel.DataAnnotations;

namespace Tgs.Authorization.ViewModels;

public class GrantRequestViewModel
{
	public required Guid FunctionId { get; set; }

	[AllowedValues(PermissionType.Allow, PermissionType.Deny)]
	public required PermissionType Type { get; set; }
}
