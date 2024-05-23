namespace Tgs.Authorization.Clients;
public record HttpFeatureSettings(
	IEnumerable<string> HttpMethod,
	string PathTemplate);