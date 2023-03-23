namespace GenericPipeline.Tests.Behaviors.UnhandledThrowingBehavior;

public class UnhandledRequestsTests
{
    public record struct RequestA : IRequest;
    public record struct RequestB : IRequest;
    public record struct UnhandledRequest : IRequest;
    public class RequestHandlerA : IRequestHandler<RequestA>
    {
        public Unit Handle(RequestA request) => throw new NotImplementedException();
    }
    public class RequestHandlerB : IRequestHandler<RequestB>
    {
        public Unit Handle(RequestB request) => throw new NotImplementedException();
    }

    [Fact]
    public void Sending_unhandled_request_can_throw_if_configured_so()
    {
        // Arrange
        var pipeline = new Pipeline()
            .AppendHandler<RequestHandlerA>()
            .AppendBehavior(new MediatorBehavior()
                .AddHandler<RequestHandlerB>())
            .ThrowOnUnhandledRequest();

        // Act
        var act = () => pipeline.Send<UnhandledRequest>(new());

        // Assert
        act.Should().ThrowExactly<UnhandledRequestException>();
    }
}
