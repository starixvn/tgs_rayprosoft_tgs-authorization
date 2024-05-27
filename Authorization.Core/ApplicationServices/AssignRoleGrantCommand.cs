namespace Tgs.Authorization.ApplicationServices;
public record AssignRoleGrantCommand(
	Guid RoleId,
	Guid FunctionId,
	PermissionType Type);
