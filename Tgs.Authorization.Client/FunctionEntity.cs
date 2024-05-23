namespace Tgs.Authorization.Clients;

record FunctionEntity(
	Guid Id,
	string Name,
	IEnumerable<SystemFunctionFeature> Features);