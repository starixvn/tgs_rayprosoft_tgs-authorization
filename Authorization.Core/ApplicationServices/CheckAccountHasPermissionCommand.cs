namespace Tgs.Authorization.ApplicationServices;
public record CheckAccountHasPermissionCommand(
	Guid AccountId,
	Guid FunctionId);
