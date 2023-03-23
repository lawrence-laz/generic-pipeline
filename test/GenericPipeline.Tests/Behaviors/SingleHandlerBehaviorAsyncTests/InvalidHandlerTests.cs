namespace GenericPipeline.Tests.Behaviors.SingleHandlerBehaviorAsyncTests;

public class InvalidHandlerTests
{
    public record struct Request : IRequest;

    public class RequestHandler : IRequestHandlerAsync<Request>
    {
        public Task<Unit> Handle(Request request) => throw new NotImplementedException();
    }

    [Fact]
    public void Sending_a_request_it_is_handled_by_the_handler()
    {
        // Arrange
        var pipeline = new PipelineAsync();

        // Act
        var act = () => pipeline.AppendHandler<RequestHandler>(handler: null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }
}

