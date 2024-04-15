using Tgs.Authorization;
using Tgs.Authorization.ApplicationServices;

namespace Authorization.Core.UnitTests;

public class CreateRoleCommandServiceTests
{
    [Fact]
    public async Task CreateRoleCommandService_建立系統角色()
    {
        // Arrange
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new CreateRoleCommandService(fakeTgsRepository);

        var command = new CreateRoleCommand("test", "role1");

        // Act
        var actual = await sut.GetAsync(command);

        // Assert
        _ = fakeTgsRepository.Received(1)
            .CreateRoleAsync(Arg.Is(new Role(actual, command.RoleName, command.System)));
    }
}
