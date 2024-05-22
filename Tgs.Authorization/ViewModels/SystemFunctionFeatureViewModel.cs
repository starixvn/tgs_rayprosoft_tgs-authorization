using System.Text.Json;

namespace Tgs.Authorization.ViewModels;

public class SystemFunctionFeatureViewModel
{
	public required Guid Id { get; set; }
	public required string Name { get; set; }
	public required string Type { get; set; }
	public required JsonDocument Settings { get; set; }
}
