using Tgs.Authorization;
using Tgs.Authorization.ApplicationServices;

namespace Authorization.Core.UnitTests;

public class RemoveRoleGrantCommandServiceTests
{
    [Fact]
    public async Task RemoveRoleGrantCommandService_移除角色權限()
    {
        // Arrange
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new RemoveRoleGrantCommandService(fakeTgsRepository);

        var command = new RemoveRoleGrantCommand(Guid.NewGuid(), Guid.NewGuid());

        // Act
        await sut.ExecuteAsync(command);

        // Assert
        _ = fakeTgsRepository.Received(1)
            .DeleteRoleFunctionGrantAsync(
                Arg.Is(command.RoleId),
                Arg.Is(command.FunctionId));
    }
}
