using Tgs.Authorization;
using Tgs.Authorization.ApplicationServices;

namespace Authorization.Core.UnitTests;
public class DeleteRoleCommandServiceTests
{
    [Fact]
    public async Task DeleteRoleCommandService_刪除角色()
    {
        // Arrange
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new DeleteRoleCommandService(fakeTgsRepository);

        var command = new DeleteRoleCommand(Guid.NewGuid());

        // Act
        await sut.ExecuteAsync(command);

        // Assert
        _ = fakeTgsRepository.Received(1)
            .DeleteRoleAsync(Arg.Is(command.RoleId));
    }
}
