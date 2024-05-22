using Tgs.Authorization;
using Tgs.Authorization.ApplicationServices;

namespace Authorization.Core.UnitTests;
public class ListSystemFunctionsQueryServiceTests
{
    [Fact]
    public async Task ListSystemFunctionsQueryService_取得系統Function定義列表()
    {
        // Arrange
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new ListSystemFunctionsQueryService(fakeTgsRepository);

        var query = new ListSystemFunctionsQuery("Test");

        // Act
        _ = await sut.QueryAsync(query).ToArrayAsync();

        // Assert
        _ = fakeTgsRepository.Received(1)
            .GetSystemFunctionsAsync(Arg.Is(query.System));
    }
}
