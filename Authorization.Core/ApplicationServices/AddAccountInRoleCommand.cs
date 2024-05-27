namespace Tgs.Authorization.ApplicationServices;
public record AddAccountInRoleCommand(Guid AccountId, Guid RoleId, int Priority);
