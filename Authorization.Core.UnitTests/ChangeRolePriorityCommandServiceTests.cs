using Tgs.Authorization;
using Tgs.Authorization.ApplicationServices;

namespace Authorization.Core.UnitTests;

public class ChangeRolePriorityCommandServiceTests
{
    [Fact]
    public async Task ChangeRolePriorityCommandService_變更角色優先權()
    {
        // Arrange
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new ChangeRolePriorityCommandService(fakeTgsRepository);

        var command = new ChangeRolePriorityCommand(Guid.NewGuid(), 100);

        // Act
        await sut.ExecuteAsync(command);

        // Assert
        _ = fakeTgsRepository.Received(1)
            .SetRolePriorityAsync(Arg.Is(command.RoleId), Arg.Is(command.NewPriority));
    }
}
