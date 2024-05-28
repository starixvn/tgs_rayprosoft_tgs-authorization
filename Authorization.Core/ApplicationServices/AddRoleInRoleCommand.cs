namespace Tgs.Authorization.ApplicationServices;
public record AddRoleInRoleCommand(
	Guid RoleId,
	Guid ParentRoleId,
	int Priority);
