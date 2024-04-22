using Tgs.Authorization;
using Tgs.Authorization.ApplicationServices;

namespace Authorization.Core.UnitTests;

public class AssignAccountGrantCommandServiceTests
{
    [Fact]
    public async Task AssignAccountGrantCommandService_設定Account權限()
    {
        // Arrange
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new AssignAccountGrantCommandService(fakeTgsRepository);

        var command = new AssignAccountGrantCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            PermissionType.Allow);

        // Act
        await sut.ExecuteAsync(command);

        // Assert
        _ = fakeTgsRepository.Received(1)
            .UpsertAccountFunctionGrantAsync(
                Arg.Is(new AccountGrant(
                    command.FunctionId,
                    command.AccountId,
                    command.Type)));
    }
}
