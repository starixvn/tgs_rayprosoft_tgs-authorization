using Tgs.Authorization;
using Tgs.Authorization.ApplicationServices;

namespace Authorization.Core.UnitTests;

public class RemoveAccountGrantCommandServiceTests
{
    [Fact]
    public async Task RemoveAccountGrantCommandService_移除帳號權限()
    {
        // Arrange
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new RemoveAccountGrantCommandService(fakeTgsRepository);

        var command = new RemoveAccountGrantCommand(Guid.NewGuid(), Guid.NewGuid());

        // Act
        await sut.ExecuteAsync(command);

        // Assert
        _ = fakeTgsRepository.Received(1)
            .DeleteAccountFunctionGrantAsync(
                Arg.Is(command.FunctionId),
                Arg.Is(command.AccountId));
    }
}
