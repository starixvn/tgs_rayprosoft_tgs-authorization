using Tgs.Authorization;
using Tgs.Authorization.ApplicationServices;

namespace Authorization.Core.UnitTests;

public class DeleteFunctionCommandServiceTests
{
    [Fact]
    public async Task DeleteFunctionCommandService_刪除SystemFunction定義()
    {
        // Arrange
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new DeleteFunctionCommandService(fakeTgsRepository);

        var command = new DeleteFunctionCommand(Guid.NewGuid());

        // Act
        await sut.ExecuteAsync(command);

        // Assert
        _ = fakeTgsRepository.Received(1)
            .DeleteFunctionAsync(Arg.Is(command.FunctionId));
    }
}
