namespace Tgs.Authorization.Clients;
public record AccountGrant(
	Guid FunctionId,
	Guid AccountId,
	PermissionType Type);