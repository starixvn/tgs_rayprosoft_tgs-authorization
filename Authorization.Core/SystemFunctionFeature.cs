namespace Tgs.Authorization;
public record SystemFunctionFeature<TFeatureSettings>(
	Guid Id,
	string Name,
	string Type,
	TFeatureSettings Settings);
