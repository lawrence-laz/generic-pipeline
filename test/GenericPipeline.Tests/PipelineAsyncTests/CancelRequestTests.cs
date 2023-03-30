using System.Diagnostics;

namespace GenericPipeline.Tests.PipelineAsyncTests;

public class CancelRequestTests
{
    public record struct LongRunningRequest : IRequest;

    public class LongRunningRequestHandler : IRequestHandlerAsync<LongRunningRequest>
    {
        public Stopwatch? Stopwatch { get; set; }

        public async Task<Unit> Handle(LongRunningRequest request, CancellationToken cancellationToken)
        {
            try
            {
                Stopwatch = Stopwatch.StartNew();
                await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken);
                return Unit.Value;
            }
            finally
            {
                Stopwatch?.Stop();
            }
        }
    }

    public class LongRunningBehavior : PipelineBehaviorAsync
    {
        public Stopwatch? Stopwatch { get; set; }
        public override async Task<TResponse> Handle<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
        {
            try
            {
                Stopwatch = Stopwatch.StartNew();
                await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken);
                return await HandleNext<TRequest, TResponse>(request, cancellationToken);
            }
            finally
            {
                Stopwatch?.Stop();
            }
        }
    }

    [Fact]
    public async Task I_can_cancel_long_running_handlers()
    {
        // Arrange
        var handler = new LongRunningRequestHandler();
        var pipeline = new PipelineAsync().AppendHandler(handler);
        var cancellationTokenSource = new CancellationTokenSource();

        // Act
        cancellationTokenSource.CancelAfter(TimeSpan.FromMilliseconds(200));
        var act = async () => await pipeline.SendAsync<LongRunningRequest>(new(), cancellationTokenSource.Token);

        // Assert
        await act.Should().ThrowAsync<TaskCanceledException>();
        handler.Stopwatch.Should().NotBeNull();
        handler.Stopwatch?.Elapsed.Should().BeLessThan(TimeSpan.FromSeconds(1));
    }

    [Fact]
    public async Task I_can_cancel_long_running_behaviors()
    {
        // Arrange
        var handler = new LongRunningBehavior();
        var pipeline = new PipelineAsync().AppendBehavior(handler);
        var cancellationTokenSource = new CancellationTokenSource();

        // Act
        cancellationTokenSource.CancelAfter(TimeSpan.FromMilliseconds(200));
        var act = async () => await pipeline.SendAsync<LongRunningRequest>(new(), cancellationTokenSource.Token);

        // Assert
        await act.Should().ThrowAsync<TaskCanceledException>();
        handler.Stopwatch.Should().NotBeNull();
        handler.Stopwatch?.Elapsed.Should().BeLessThan(TimeSpan.FromSeconds(1));
    }
}

