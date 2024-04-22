using Tgs.Authorization.ViewModels.Annotations;

namespace Tgs.Authorization.ViewModels;

public class RoleGrantRequestViewModel : GrantRequestViewModel
{
	[ValueRange<int>(0, 100000)]
	public required int Priority { get; set; } = 100;
}
