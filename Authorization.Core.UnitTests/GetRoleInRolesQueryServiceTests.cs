using Tgs.Authorization;
using Tgs.Authorization.ApplicationServices;

namespace Authorization.Core.UnitTests;

public class GetRoleInRolesQueryServiceTests
{
    [Fact]
    public async Task GetRoleInRolesQueryService_取得歸屬在此角色下的所有角色Id()
    {
        // Arrange
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new GetChildrenRolesQueryService(fakeTgsRepository);

        var query = new GetChildrenRolesQuery(Guid.NewGuid());

        // Act
        await sut.QueryAsync(query).ToArrayAsync();

        // Assert
        _ = fakeTgsRepository.Received(1)
            .GetChildrenRolesAsync(Arg.Is(query.RoleId));
    }
}