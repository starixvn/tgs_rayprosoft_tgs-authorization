namespace Tgs.Authorization.ApplicationServices;
public record RemoveAccountGrantCommand(Guid AccountId, Guid FunctionId);
