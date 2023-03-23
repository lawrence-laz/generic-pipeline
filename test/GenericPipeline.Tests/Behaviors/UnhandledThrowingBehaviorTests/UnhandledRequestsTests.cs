namespace GenericPipeline.Tests.Behaviors.UnhandledThrowingBehavior;

public class UnhandledRequestsTests
{
    public record struct Request : IRequest;

    [Fact]
    public void Sending_unhandled_request_can_throw_if_configured_so()
    {
        // Arrange
        var pipeline = new Pipeline()
            .ThrowOnUnhandledRequest();

        // Act
        var act = () => pipeline.Send<Request>(new());

        // Assert
        act.Should().ThrowExactly<UnhandledRequestException>();
    }
}
