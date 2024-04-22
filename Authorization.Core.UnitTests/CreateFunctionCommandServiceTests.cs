using Tgs.Authorization;
using Tgs.Authorization.ApplicationServices;

namespace Authorization.Core.UnitTests;

public class CreateFunctionCommandServiceTests
{
    [Fact]
    public async Task CreateFunctionCommandService_定義SystemFunction()
    {
        // Arrange
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new CreateFunctionCommandService(fakeTgsRepository);

        var command = new CreateFunctionCommand("function1", "test");

        // Act
        var actual = await sut.GetAsync(command);

        // Assert
        _ = fakeTgsRepository.Received(1)
            .CreateFunctionAsync(
                Arg.Is(new SystemFunction(
                    actual,
                    command.FunctionName,
                    command.System)));
    }
}
