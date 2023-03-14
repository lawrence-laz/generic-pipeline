namespace GenericPipeline.Tests.Behaviors.SingleHandlerBehaviorTests;

public record struct RequestA : IRequest;
public record struct RequestB : IRequest;

public class RequestBHandler : IRequestHandler<RequestB>
{
    public Unit Handle(RequestB request) => Unit.Value;
}

public class InvalidHandlerTests
{
    [Fact]
    public void fewfwef()
    {
        // Arrange
        var pipeline = new Pipeline()
            .AppendHandler<RequestBHandler>(new()
            {
                ThrowUhandledRequestType = true
            });

        // Act
        var act = () => pipeline.Send<RequestA, Unit>(new());

        // Assert
        act.Should().Throw<InvalidOperationException>(
            $"because {nameof(RequestBHandler)} does not handle {nameof(RequestA)} " +
            $"and {nameof(HandlerOptions.ThrowUhandledRequestType)} is set to true.");
    }
}

