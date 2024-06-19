namespace Tgs.Authorization.ApplicationServices;
public record ChangeRolePriorityCommand(
	Guid RoleId,
	int NewPriority);
