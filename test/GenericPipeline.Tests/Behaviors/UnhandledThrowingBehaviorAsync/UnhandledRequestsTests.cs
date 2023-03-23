namespace GenericPipeline.Tests.Behaviors.UnhandledThrowingBehaviorAsync;

public class UnhandledRequestsTests
{
    public record struct Request : IRequest;

    [Fact]
    public async Task Sending_unhandled_request_can_throw_if_configured_so()
    {
        // Arrange
        var pipeline = new PipelineAsync()
            .ThrowOnUnhandledRequest();

        // Act
        var act = async () => await pipeline.SendAsync<Request>(new());

        // Assert
        await act.Should().ThrowExactlyAsync<UnhandledRequestException>();
    }
}
