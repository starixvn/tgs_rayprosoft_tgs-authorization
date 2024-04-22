using Tgs.Authorization.ViewModels.Annotations;

namespace Tgs.Authorization.UnitTests;

public class ValueRangeAttributeTests
{
	[Fact]
	public void ValueRangeAttribute_如果值為空略過檢查()
	{
		// Arrange
		var sut = new ValueRangeAttribute<int>(0, 100);

		// Act
		var actual = sut.IsValid(null);

		// Assert
		Assert.True(actual);
	}

	[Fact]
	public void ValueRangeAttribute_如果值的型別不是檢查的型別_回傳檢查失敗()
	{
		// Arrange
		var sut = new ValueRangeAttribute<int>(0, 100);

		// Act
		var actual = sut.IsValid(50L);

		// Assert
		Assert.False(actual);
	}

	[Fact]
	public void ValueRangeAttribute_如果值的型別是檢查的型別_並且值在範圍內_回傳檢查通過()
	{
		// Arrange
		var sut = new ValueRangeAttribute<int>(0, 100);

		// Act
		var actual = sut.IsValid(50);

		// Assert
		Assert.True(actual);
	}

	[Fact]
	public void ValueRangeAttribute_值的檢查範圍是大於等於Minimum_並且小於Maximum()
	{
		// Arrange
		var sut = new ValueRangeAttribute<int>(0, 100);

		// Assert
		Assert.True(sut.IsValid(0));
		Assert.False(sut.IsValid(-1));
		Assert.True(sut.IsValid(50));
		Assert.False(sut.IsValid(100));
	}
}
