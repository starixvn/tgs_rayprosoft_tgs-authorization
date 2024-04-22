using Tgs.Authorization;
using Tgs.Authorization.ApplicationServices;

namespace Authorization.Core.UnitTests;

public class AssignRoleGrantCommandServiceTests
{
    [Fact]
    public async Task AssignRoleGrantCommandService_設定角色權限()
    {
        // Arrange
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new AssignRoleGrantCommandService(fakeTgsRepository);

        var command = new AssignRoleGrantCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            PermissionType.Allow,
            100);

        // Act
        await sut.ExecuteAsync(command);

        // Assert
        _ = fakeTgsRepository.Received(1)
            .UpsertRoleFunctionGrantAsync(
                Arg.Is(new RoleGrant(
                    command.FunctionId,
                    command.RoleId,
                    command.Type,
                    command.Priority)));
    }
}
