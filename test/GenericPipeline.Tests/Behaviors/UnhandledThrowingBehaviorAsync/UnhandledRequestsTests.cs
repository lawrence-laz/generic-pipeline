namespace GenericPipeline.Tests.Behaviors.UnhandledThrowingBehaviorAsync;

public class UnhandledRequestsTests
{
    public record struct RequestA : IRequest;
    public record struct RequestB : IRequest;
    public record struct UnhandledRequest : IRequest;
    public class RequestHandlerA : IRequestHandler<RequestA>
    {
        public void Handle(RequestA request) => throw new NotImplementedException();
    }
    public class RequestHandlerB : IRequestHandler<RequestB>
    {
        public void Handle(RequestB request) => throw new NotImplementedException();
    }

    [Fact]
    public async Task Sending_unhandled_request_can_throw_if_configured_so()
    {
        // Arrange
        var pipeline = new PipelineAsync()
            .AppendHandler<RequestHandlerA>()
            .AppendBehavior(new MediatorBehaviorAsync()
                .AddHandler<RequestHandlerB>())
            .ThrowOnUnhandledRequest();

        // Act
        var act = async () => await pipeline.SendAsync<UnhandledRequest>(new());

        // Assert
        await act.Should().ThrowExactlyAsync<UnhandledRequestException>();
    }
}
