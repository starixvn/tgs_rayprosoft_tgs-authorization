using Tgs.Authorization;
using Tgs.Authorization.ApplicationServices;

namespace Authorization.Core.UnitTests;

public class RenameFunctionCommandServiceTests
{
    [Fact]
    public async Task RenameFunctionCommandService_更改SystemFunction名稱()
    {
        // Arrange
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new RenameFunctionCommandService(fakeTgsRepository);

        var command = new RenameFunctionCommand(Guid.NewGuid(), "newName");

        // Act
        await sut.ExecuteAsync(command);

        // Assert
        _ = fakeTgsRepository.Received(1)
            .RenameFunctionAsync(
                Arg.Is(command.FunctionId),
                Arg.Is(command.NewName));
    }
}
