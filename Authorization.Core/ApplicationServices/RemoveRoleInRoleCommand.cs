namespace Tgs.Authorization.ApplicationServices;
public record RemoveRoleInRoleCommand(
	Guid RoleId,
	Guid ParentRoleId);
