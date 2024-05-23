using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tgs.Authorization.Clients;

internal static class ClientUtility
{
	public static readonly JsonSerializerOptions SerializerOptions = new()
	{
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		WriteIndented = true,
		Converters = {
			new JsonStringEnumConverter()
		}
	};
}