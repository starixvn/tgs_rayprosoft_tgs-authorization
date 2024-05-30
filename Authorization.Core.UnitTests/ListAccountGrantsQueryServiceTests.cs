using Tgs.Authorization;
using Tgs.Authorization.ApplicationServices;

namespace Authorization.Core.UnitTests;

public class ListAccountGrantsQueryServiceTests
{
    [Fact]
    public async Task ListAccountGrantsQueryService_取得Account授權設定列表()
    {
        // Arrange
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new ListAccountGrantsQueryService(fakeTgsRepository);

        var query = new ListAccountGrantsQuery(Guid.NewGuid());

        // Act
        await sut.QueryAsync(query).ToArrayAsync();

        // Assert
        _ = fakeTgsRepository.Received(1)
            .GetAccountFunctionGrantsAsync(Arg.Is(query.AccountId));
    }
}