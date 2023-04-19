namespace GenericPipeline.Tests.Behaviors.MediatorBehaviorAsyncTests;

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
    public class RequestHandlerAsync : IRequestHandlerAsync<Request>
    {
        public Task Handle(Request request, CancellationToken cancellationToken) => throw new NotImplementedException();
    }

    [Fact]
    public void Adding_same_handler_twice_throws()
    {
        // Arrange
        var mediator = new MediatorBehaviorAsync()
            .AddHandler<RequestHandler>();

        // Act
        var act = () => mediator.AddHandler<RequestHandler>();

        // Assert
        act.Should().Throw<DuplicateHandlerException>();
    }

    [Fact]
    public void Adding_same_request_async_non_async_handlers_throws()
    {
        // Arrange
        var mediator = new MediatorBehaviorAsync()
            .AddHandler<RequestHandler>();

        // Act
        var act = () => mediator.AddHandler<RequestHandlerAsync>();

        // Assert
        act.Should().Throw<DuplicateHandlerException>();
    }


    [Fact]
    public void Adding_different_handlers_handling_same_request_throws()
    {
        // Arrange
        var mediator = new MediatorBehaviorAsync()
            .AddHandler<RequestHandler>();

        // Act
        var act = () => mediator.AddHandler<OtherRequestHandler>();

        // Assert
        act.Should().Throw<DuplicateHandlerException>();
    }
}
