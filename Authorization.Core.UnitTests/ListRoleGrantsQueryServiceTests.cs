using Tgs.Authorization;
using Tgs.Authorization.ApplicationServices;

namespace Authorization.Core.UnitTests;
public class ListRoleGrantsQueryServiceTests
{
    [Fact]
    public async Task ListRoleGrantsQueryService_取得角色權限設定列表()
    {
        // Arrange
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new ListRoleGrantsQueryService(fakeTgsRepository);

        var query = new ListRoleGrantsQuery(Guid.NewGuid());

        // Act
        _ = await sut.QueryAsync(query).ToArrayAsync();

        // Assert
        _ = fakeTgsRepository.Received(1)
            .GetRoleFunctionGrantsAsync(Arg.Is(query.RoleId));
    }
}
