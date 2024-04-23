using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tgs.Authorization;

public static class AuthorizationUtility
{
	public static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web)
	{
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		WriteIndented = false,
		DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
		Converters = {
		}
	};
}
