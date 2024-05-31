namespace Tgs.Authorization;
public record RoleGrant(
	Guid FunctionId,
	Guid RoleId,
	PermissionType Type);
