using Tgs.Authorization;
using Tgs.Authorization.ApplicationServices;

namespace Authorization.Core.UnitTests;

public class GetSystemAllRolesQueryServiceTests
{
    [Fact]
    public async Task GetSystemAllRolesQueryService_取得系統所有定義的角色()
    {
        // Arrange
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new GetSystemAllRolesQueryService(fakeTgsRepository);

        var query = new GetSystemAllRolesQuery("test");

        // Act
        _ = await sut.QueryAsync(query).ToArrayAsync();

        // Assert
        _ = fakeTgsRepository.Received(1)
            .GetSystemAllRolesAsync(Arg.Is(query.System));
    }
}
