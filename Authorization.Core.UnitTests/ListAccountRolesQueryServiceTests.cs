using Tgs.Authorization;
using Tgs.Authorization.ApplicationServices;

namespace Authorization.Core.UnitTests;

public class ListAccountRolesQueryServiceTests
{
    [Fact]
    public async Task ListAccountRolesQueryService_列表帳號所屬角色()
    {
        // Arrange
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new ListAccountRolesQueryService(fakeTgsRepository);

        var query = new ListAccountRolesQuery(Guid.NewGuid());

        // Act
        _ = await sut.QueryAsync(query).ToArrayAsync();

        // Assert
        _ = fakeTgsRepository.Received(1)
            .GetAccountAllRolesAsync(
                Arg.Is(query.AccountId),
                Arg.Is(false));
    }
}