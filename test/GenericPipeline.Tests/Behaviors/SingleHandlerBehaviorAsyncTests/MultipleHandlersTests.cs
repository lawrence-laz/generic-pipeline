namespace GenericPipeline.Tests.Behaviors.SingleHandlerBehaviorAsyncTests;

public class MultipleHandlersTests
{
    public record struct RequestA : IRequest;
    public record struct RequestB : IRequest;

    public class RequestAHandler : IRequestHandlerAsync<RequestA>
    {
        public int InvocationsCount { get; set; }

        public Task Handle(RequestA request, CancellationToken cancellationToken)
        {
            ++InvocationsCount;
            return Task.CompletedTask;
        }
    }

    public class RequestBHandler : IRequestHandlerAsync<RequestB>
    {
        public int InvocationsCount { get; set; }

        public async Task Handle(RequestB request, CancellationToken cancellationToken)
        {
            ++InvocationsCount;
            await Task.Yield();
        }
    }

    [Fact]
    public async Task Sending_first_request_it_is_handled_by_first_handler_and_second_handler_is_not_called()
    {
        // Arrange
        var pipeline = new PipelineAsync()
            .AppendHandler<RequestAHandler>()
            .AppendHandler<RequestBHandler>();

        // Act
        await pipeline.SendAsync<RequestA>(new());

        // Assert
        pipeline.GetHandler<RequestAHandler>().InvocationsCount.Should().Be(1);
        pipeline.GetHandler<RequestBHandler>().InvocationsCount.Should().Be(0);
    }

    [Fact]
    public async Task Sending_second_request_it_is_handled_by_second_handler_and_first_handler_is_not_called()
    {
        // Arrange
        var pipeline = new PipelineAsync()
            .AppendHandler<RequestAHandler>()
            .AppendHandler<RequestBHandler>();

        // Act
        await pipeline.SendAsync<RequestB>(new());

        // Assert
        pipeline.GetHandler<RequestAHandler>().InvocationsCount.Should().Be(0);
        pipeline.GetHandler<RequestBHandler>().InvocationsCount.Should().Be(1);
    }
}

