using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tgs.Authorization.Clients;

internal class AuthorizationClient(HttpClient httpClient)
{
	private static readonly JsonSerializerOptions _SerializerOptions = new()
	{
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		WriteIndented = true,
		Converters = {
			new JsonStringEnumConverter()
		}
	};

	public IAsyncEnumerable<SystemFunction> ListSystemFunctionsAsync(string system, CancellationToken cancellationToken = default)
		=> httpClient.GetAsync($"/function/{system}", cancellationToken)
			.ToAsyncEnumerable()
			.Do(response => response.EnsureSuccessStatusCode())
			.SelectMany(response => response.Content
				.ReadFromJsonAsAsyncEnumerable<SystemFunction>(_SerializerOptions, cancellationToken))
			.Where(sysFunc => sysFunc is not null)
			.Select(sysFunc => sysFunc!);

	public IAsyncEnumerable<SystemFunctionFeature<TFeatureSettings>> ListSystemFunctionFeaturesAsync<TFeatureSettings>(
		Guid functionId,
		CancellationToken cancellationToken = default)
		=> httpClient.GetAsync($"/function/feature/{functionId}", cancellationToken)
			.ToAsyncEnumerable()
			.Do(response => response.EnsureSuccessStatusCode())
			.SelectMany(response => response.Content
				.ReadFromJsonAsAsyncEnumerable<SystemFunctionFeature<TFeatureSettings>>(_SerializerOptions, cancellationToken))
			.Where(feat => feat is not null)
			.Select(feat => feat!);
}