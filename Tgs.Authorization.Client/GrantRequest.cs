namespace Tgs.Authorization.Clients;
public record GrantRequest(Guid FunctionId, PermissionType Type);