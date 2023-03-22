namespace GenericPipeline.Tests.Behaviors.SingleHandlerBehaviorAsyncTests;

public class OneHandlerTests
{
    public record struct Request : IRequest;

    public class RequestHandler : IRequestHandlerAsync<Request>
    {
        public int InvocationsCount { get; set; }

        public Task<Unit> Handle(Request request)
        {
            ++InvocationsCount;
            return Task.FromResult(Unit.Value);
        }
    }

    [Fact]
    public async Task Sending_a_request_it_is_handled_by_the_handler()
    {
        // Arrange
        var pipeline = new PipelineAsync()
            .AppendHandler<RequestHandler>();

        // Act
        await pipeline.SendAsync<Request>(new());

        // Assert
        pipeline.GetHandler<RequestHandler>().InvocationsCount.Should().Be(1);
    }
}

