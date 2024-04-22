namespace Tgs.Authorization.ApplicationServices;
public record AssignAccountGrantCommand(
	Guid AccountId,
	Guid FunctionId,
	PermissionType Type);
