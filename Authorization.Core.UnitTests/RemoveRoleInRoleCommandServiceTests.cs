using Tgs.Authorization;
using Tgs.Authorization.ApplicationServices;

namespace Authorization.Core.UnitTests;
public class RemoveRoleInRoleCommandServiceTests
{
    [Fact]
    public async Task RemoveRoleInRoleCommandService_從角色中把包含的角色移除()
    {
        // Arrange
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new RemoveRoleInRoleCommandService(fakeTgsRepository);

        var command = new RemoveRoleInRoleCommand(Guid.NewGuid(), Guid.NewGuid());

        // Act
        await sut.ExecuteAsync(command);

        // Assert
        _ = fakeTgsRepository.Received(1)
            .RemoveRoleInRoleAsync(Arg.Is(command.RoleId), Arg.Is(command.ParentRoleId));
    }
}
