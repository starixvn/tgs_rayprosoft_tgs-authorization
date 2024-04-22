namespace Tgs.Authorization.ApplicationServices;
public record RenameFunctionCommand(Guid FunctionId, string NewName);
