namespace GenericPipeline.Tests.Behaviors.MediatorBehaviorTests;

public class DuplicateHandlersTests
{
    public record struct Request : IRequest;
    public class RequestHandler : IRequestHandler<Request>
    {
        public void Handle(Request request) => throw new NotImplementedException();
    }
    public class OtherRequestHandler : IRequestHandler<Request>
    {
        public void Handle(Request request) => throw new NotImplementedException();
    }

    [Fact]
    public void Adding_same_handler_twice_throws()
    {
        // Arrange
        var mediator = new MediatorBehavior()
            .AddHandler<RequestHandler>();

        // Act
        var act = () => mediator.AddHandler<RequestHandler>();

        // Assert
        act.Should().Throw<DuplicateHandlerException>();
    }

    [Fact]
    public void Adding_different_handlers_handling_same_request_throws()
    {
        // Arrange
        var mediator = new MediatorBehavior()
            .AddHandler<RequestHandler>();

        // Act
        var act = () => mediator.AddHandler<OtherRequestHandler>();

        // Assert
        act.Should().Throw<DuplicateHandlerException>();
    }
}
