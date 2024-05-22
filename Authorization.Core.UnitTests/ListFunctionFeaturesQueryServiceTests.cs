using Tgs.Authorization;
using Tgs.Authorization.ApplicationServices;

namespace Authorization.Core.UnitTests;

public class ListFunctionFeaturesQueryServiceTests
{
    [Fact]
    public async Task ListFunctionFeaturesQueryService_取得Function特徵設定列表()
    {
        // Arrange
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new ListFunctionFeaturesQueryService(fakeTgsRepository);

        var query = new ListFunctionFeaturesQuery(Guid.NewGuid());

        // Act
        _ = await sut.QueryAsync(query).ToArrayAsync();

        // Assert
        _ = fakeTgsRepository.Received(1)
            .GetSystemFunctionFeaturesAsync(Arg.Is(query.FunctionId));
    }
}