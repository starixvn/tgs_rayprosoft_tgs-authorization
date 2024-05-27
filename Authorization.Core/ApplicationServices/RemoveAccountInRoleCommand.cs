namespace Tgs.Authorization.ApplicationServices;
public record RemoveAccountInRoleCommand(Guid AccountId, Guid RoleId);