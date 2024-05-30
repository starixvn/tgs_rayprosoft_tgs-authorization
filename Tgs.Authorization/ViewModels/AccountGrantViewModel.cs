namespace Tgs.Authorization.ViewModels;

public class AccountGrantViewModel
{
	public required Guid FunctionId { get; set; }
	public required Guid AccountId { get; set; }
	public required PermissionType Type { get; set; }
}