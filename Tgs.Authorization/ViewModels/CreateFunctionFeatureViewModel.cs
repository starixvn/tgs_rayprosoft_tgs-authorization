using System.Text.Json;

namespace Tgs.Authorization.ViewModels;

public class CreateFunctionFeatureViewModel
{
	public required string Name { get; set; }
	public required string Type { get; set; }
	public required JsonDocument Settings { get; set; }
}
