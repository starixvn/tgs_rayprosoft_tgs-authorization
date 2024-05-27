using Tgs.Authorization;
using Tgs.Authorization.ApplicationServices;

namespace Authorization.Core.UnitTests;
public class AddAccountInRoleCommandServiceTests
{
    [Fact]
    public async Task AddAccountInRoleCommandService_將Account加到Role中()
    {
        // Arrange
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new AddAccountInRoleCommandService(fakeTgsRepository);

        var command = new AddAccountInRoleCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            100);

        // Act
        await sut.ExecuteAsync(command);

        // Assert
        _ = fakeTgsRepository.Received(1)
            .UpsertAccountInRoleAsync(
                Arg.Is(command.AccountId),
                Arg.Is(command.RoleId),
                Arg.Is(command.Priority));
    }
}
