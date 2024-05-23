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
	{
		if (Type == FeatureType.Http && typeof(TSettings) == typeof(HttpFeatureSettings))
			return Settings.Deserialize<TSettings>(ClientUtility.SerializerOptions);

		return null;
	}
}