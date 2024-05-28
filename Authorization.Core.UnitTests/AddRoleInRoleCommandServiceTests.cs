using Tgs.Authorization;
using Tgs.Authorization.ApplicationServices;

namespace Authorization.Core.UnitTests;

public class AddRoleInRoleCommandServiceTests
{
    [Fact]
    public async Task AddRoleInRoleCommandService_指定角色到另一個角色中()
    {
        // Arrange
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new AddRoleInRoleCommandService(fakeTgsRepository);

        var command = new AddRoleInRoleCommand(Guid.NewGuid(), Guid.NewGuid(), 100);

        // Act
        await sut.ExecuteAsync(command);

        // Assert
        _ = fakeTgsRepository.Received(1)
            .UpsertRoleInRoleAsync(
                Arg.Is(command.RoleId),
                Arg.Is(command.ParentRoleId),
                Arg.Is(command.Priority));
    }
}
