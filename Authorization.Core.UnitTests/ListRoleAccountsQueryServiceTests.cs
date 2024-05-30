using Tgs.Authorization;
using Tgs.Authorization.ApplicationServices;

namespace Authorization.Core.UnitTests;
public class ListRoleAccountsQueryServiceTests
{
    [Fact]
    public async Task ListRoleAccountsQueryService_取得角色Account列表()
    {
        // Arrange
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new ListRoleAccountsQueryService(fakeTgsRepository);

        var query = new ListRoleAccountsQuery(Guid.NewGuid());

        // Act
        _ = await sut.QueryAsync(query).ToArrayAsync();

        // Assert
        _ = fakeTgsRepository.Received(1)
            .GetRoleAccountsAsync(Arg.Is(query.RoleId));
    }
}
