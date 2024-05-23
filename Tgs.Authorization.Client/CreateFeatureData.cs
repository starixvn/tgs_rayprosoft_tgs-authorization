using System.Text.Json;

namespace Tgs.Authorization.Clients;
public record CreateFeatureData(
	string Name,
	FeatureType Type,
	JsonDocument Settings);