using Tgs.Authorization;
using Tgs.Authorization.ApplicationServices;

namespace Authorization.Core.UnitTests;

public class RemoveFunctionFeatureCommandServiceTests
{
    [Fact]
    public async Task RemoveFunctionFeatureCommandService_移除功能特徵()
    {
        // Arrange
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new RemoveFunctionFeatureCommandService(fakeTgsRepository);

        var command = new RemoveFunctionFeatureCommand(Guid.NewGuid());

        // Act
        await sut.ExecuteAsync(command);

        // Assert
        _ = fakeTgsRepository.Received(1)
            .DeleteFunctionFeatureAsync(Arg.Is(command.FeatureId));
    }
}
