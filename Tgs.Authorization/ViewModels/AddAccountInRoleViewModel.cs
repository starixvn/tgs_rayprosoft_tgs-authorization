namespace Tgs.Authorization.ViewModels;

public class AddAccountInRoleViewModel
{
	public required Guid RoleId { get; set; }
	public required int Priority { get; set; }
}