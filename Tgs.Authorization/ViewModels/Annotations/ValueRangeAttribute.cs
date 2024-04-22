using System.ComponentModel.DataAnnotations;

namespace Tgs.Authorization.ViewModels.Annotations;

public class ValueRangeAttribute<TValue>(TValue minimum, TValue maximum)
	: ValidationAttribute
	where TValue : IComparable<TValue>
{
	public override bool IsValid(object? value)
		=> value is null
			|| value is TValue typedValue
			&& typedValue.CompareTo(minimum) >= 0
			&& typedValue.CompareTo(maximum) < 0;
}
