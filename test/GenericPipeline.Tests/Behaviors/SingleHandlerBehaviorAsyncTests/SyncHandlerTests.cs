namespace GenericPipeline.Tests.Behaviors.SingleHandlerBehaviorAsyncTests;

public class SyncHandlerTests
{
    public record struct Request : IRequest<string>;

    public class RequestHandler : IRequestHandler<Request, string>
    {
        public string Expected { get; set; }

        public RequestHandler(string expected)
        {
            Expected = expected;
        }

        public int InvocationsCount { get; set; }

        public string Handle(Request request)
        {
            ++InvocationsCount;
            return Expected;
        }
    }

    [Theory, AutoData]
    public async Task Sending_a_request_to_async_pipeline_with_sync_handler(string expected)
    {
        // Arrange
        var handler = new RequestHandler(expected);
        var pipeline = new PipelineAsync()
            .AppendHandler(handler);

        // Act
        var actual = await pipeline.SendAsync<Request, string>(new());

        // Assert
        actual.Should().Be(expected);
        handler.InvocationsCount.Should().Be(1);
    }
}

