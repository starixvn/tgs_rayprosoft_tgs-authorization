using System.Text.Json;

namespace Tgs.Authorization.ApplicationServices;
public record AddFunctionFeatureCommand(
	Guid FunctionId,
	string FeatureName,
	string FeatureType,
	JsonDocument FeatureSettings);
