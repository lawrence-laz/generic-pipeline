namespace GenericPipeline.Tests.PipelineAsyncTests;

public class SendRequestsTests
{
    public record struct RequestA() : IRequest;
    public record struct RequestB() : IRequest<int>;

    public class TestHandler :
        IRequestHandlerAsync<RequestA>,
        IRequestHandler<RequestB, int>
    {
        public Task<Unit> Handle(RequestA request) => Unit.ValueTask;
        public int Handle(RequestB request) => 0;
    }

    public class TestBehavior : PipelineBehaviorAsync
    {
        public int InvocationsCount { get; set; }

        public override async Task<TResponse> Handle<TRequest, TResponse>(TRequest request)
        {
            ++InvocationsCount;
            return await HandleNext<TRequest, TResponse>(request);
        }
    }

    [Fact]
    public async Task Send_without_behaviors_throws()
    {
        // Arrange
        var pipeline = new PipelineAsync();

        // Act
        var actWithoutReturn = async () => await pipeline.SendAsync<RequestA>(new());
        var actWithReturn = async () => await pipeline.SendAsync<RequestB, int>(new());

        // Assert
        await actWithoutReturn.Should().ThrowAsync<InvalidOperationException>();
        await actWithReturn.Should().ThrowAsync<InvalidOperationException>();
    }


    [Fact]
    public async Task Send_without_handlers_returns_default()
    {
        // Arrange
        var pipeline = new PipelineAsync()
            .AppendBehavior<TestBehavior>();

        // Act
        var actual = await pipeline.SendAsync<RequestB, int>(new());

        // Assert
        actual.Should().Be(default);
    }
}

