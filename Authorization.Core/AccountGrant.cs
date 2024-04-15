namespace Tgs.Authorization;
public record AccountGrant(
	Guid FunctionId,
	Guid AccountId,
	PermissionType Type);
