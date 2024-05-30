namespace Tgs.Authorization.Clients;
public record RoleGrant(
	Guid FunctionId,
	Guid RoleId,
	PermissionType Type);