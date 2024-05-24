namespace Tgs.Authorization.Clients;
public record RoleGrantRequest(
	Guid FunctionId,
	PermissionType Type,
	int Priority);