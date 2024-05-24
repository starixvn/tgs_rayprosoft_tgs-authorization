using System.Text.Json;

namespace Tgs.Authorization.Clients;

public record SystemFunctionFeature(
	Guid Id,
	string Name,
	FeatureType Type,
	JsonDocument Settings)
{
	public TSettings? GetSettings<TSettings>()
		where TSettings : class
		=> Type == FeatureType.Http && typeof(TSettings) == typeof(HttpFeatureSettings)
			? Settings.Deserialize<TSettings>(ClientUtility.SerializerOptions)
			: null;
}