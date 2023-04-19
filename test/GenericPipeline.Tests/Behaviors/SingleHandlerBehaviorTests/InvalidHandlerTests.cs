namespace GenericPipeline.Tests.Behaviors.SingleHandlerBehaviorTests;

public class InvalidHandlerTests
{
    public record struct Request : IRequest;

    public class RequestHandler : IRequestHandler<Request>
    {
        public void Handle(Request request) { }
    }

    [Fact]
    public void Sending_a_request_it_is_handled_by_the_handler()
    {
        // Arrange
        var pipeline = new Pipeline();

        // Act
        var act = () => pipeline.AppendHandler<RequestHandler>(handler: null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }
}

