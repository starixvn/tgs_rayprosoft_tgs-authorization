using System.Text.Json;
using Tgs.Authorization;
using Tgs.Authorization.ApplicationServices;

namespace Authorization.Core.UnitTests;

public class AddFunctionFeatureCommandServiceTests
{
    [Fact]
    public async Task AddFunctionFeatureCommandService_新增功能特徵()
    {
        // Arrange
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new AddFunctionFeatureCommandService(fakeTgsRepository);

        var command = new AddFunctionFeatureCommand(
            Guid.NewGuid(),
            "feature1",
            "Http",
            JsonDocument.Parse("{}"));

        // Act
        var actual = await sut.GetAsync(command);

        // Assert
        _ = fakeTgsRepository.Received(1)
            .InsertFunctionFeatureAsync(
                Arg.Is(command.FunctionId),
                Arg.Is(new SystemFunctionFeature<string>(
                    actual,
                    command.FeatureName,
                    command.FeatureType,
                    "{}")));
    }
}
