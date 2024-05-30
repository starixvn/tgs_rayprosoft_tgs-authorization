namespace Tgs.Authorization.ViewModels;

public class RoleGrantViewModel
{
	public required Guid FunctionId { get; set; }
	public required Guid RoleId { get; set; }
	public required PermissionType Type { get; set; }
}
