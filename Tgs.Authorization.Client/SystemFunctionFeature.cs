using Tgs.Authorization;
namespace Tgs.Authorization.Clients;
public record SystemFunctionFeature<TFeatureSettings>(
	Guid Id,
	string Name,
	FeatureType Type,
	TFeatureSettings Settings);
