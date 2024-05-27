using Tgs.Authorization;
using Tgs.Authorization.ApplicationServices;

namespace Authorization.Core.UnitTests;

public class RemoveAccountInRoleCommandServiceTests
{
    [Fact]
    public async Task RemoveAccountInRoleCommandService_從Role中移除Account()
    {
        // Arrange
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new RemoveAccountInRoleCommandService(fakeTgsRepository);

        var command = new RemoveAccountInRoleCommand(Guid.NewGuid(), Guid.NewGuid());

        // Act
        await sut.ExecuteAsync(command);

        // Assert
        _ = fakeTgsRepository.Received(1)
            .RemoveAccountInRoleAsync(Arg.Is(command.AccountId), Arg.Is(command.RoleId));
    }
}