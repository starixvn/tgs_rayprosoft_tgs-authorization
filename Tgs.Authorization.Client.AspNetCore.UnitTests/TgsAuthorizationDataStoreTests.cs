using Microsoft.Extensions.DependencyInjection;
using Tgs.Authorization.Clients;

namespace Tgs.Authorization.Client.AspNetCore.UnitTests;

public class TgsAuthorizationDataStoreTests
{
	[Fact]
	public async Task TgsAuthorizationDataStore_取得系統功能設定列表()
	{
		// Arrange
		var fakeAuthorizationClient = Substitute.For<IAuthorizationClient>();
		var serviceProvider = new ServiceCollection()
			.BuildServiceProvider();

		var sut = new TgsAuthorizationDataStore(fakeAuthorizationClient, serviceProvider);

		var system = "Test";

		// Act
		_ = await sut.GetFunctionsAsync(system).ToArrayAsync();

		// Assert
		_ = fakeAuthorizationClient.Received(1)
			.ListSystemFunctionsAsync(Arg.Is(system));
	}

	[Fact]
	public async Task TgsAuthorizationDataStore_取得指定名稱的功能設定()
	{
		// Arrange
		var fakeAuthorizationClient = Substitute.For<IAuthorizationClient>();
		var serviceProvider = new ServiceCollection()
			.BuildServiceProvider();

		var sut = new TgsAuthorizationDataStore(fakeAuthorizationClient, serviceProvider);

		var system = "Test";
		var functionName = "func1";

		_ = fakeAuthorizationClient.ListSystemFunctionsAsync(Arg.Is(system))
			.Returns(new[]
			{
				new SystemFunction(Guid.NewGuid(), functionName, system)
			}.ToAsyncEnumerable());

		// Act
		var actual = await sut.FindFunctionAsync(system, functionName);

		// Assert
		Assert.NotNull(actual);
	}
}