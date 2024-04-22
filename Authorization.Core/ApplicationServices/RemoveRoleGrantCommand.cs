namespace Tgs.Authorization.ApplicationServices;
public record RemoveRoleGrantCommand(Guid RoleId, Guid FunctionId);
